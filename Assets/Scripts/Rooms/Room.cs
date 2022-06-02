using System.Collections.Generic;
using Enemies;
using GoldProject.FrighteningEvent;
using GridSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace GoldProject.Rooms
{
    [System.Serializable]
    public class Room
    {
        public string name;
        public bool isCorridor;
        public Vector2 Position => roomTransform.position;
        public Vector2Int size;

        public bool IsInside(Vector2 worldPosition)
        {
            Vector2 position = Position;
            float halfLength = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;
            return position.x - halfLength < worldPosition.x &&
                   worldPosition.x < position.x + halfLength &&
                   position.y - halfHeight < worldPosition.y &&
                   worldPosition.y < position.y + halfHeight;
        }

        private bool lighten;
        public bool IsLighten => lighten;

        [HideInInspector] public Curtain[] curtains;
        [HideInInspector] public FrighteningEventBase[] frighteningEvents;
        [HideInInspector] public List<Garlic> garlics;

        /*[HideInInspector]*/ public List<Enemies.EnemyBase> enemies = new List<EnemyBase>();
        public Transform[] pathPoints;

        [FormerlySerializedAs("roomCollidersTransform"), Header("Colliders"),
         Tooltip("GameObject contaning all the colliders of the room")]
        public Transform roomTransform;

        private Collider2D[] roomColliders;

        public void Initialize()
        {
            if (!roomTransform)
            {
                Debug.LogWarning($"Room named {name} doesn't have a transform");
                return;
            } 
            
            // Initialize curtains
            curtains = roomTransform.GetComponentsInChildren<Curtain>();
            foreach (Curtain curtain in curtains)
            {
                if (curtain == null)
                    continue;
                
                curtain.SetOpened(false);
                curtain.onStateChanged = UpdateLightState;
            }

            // Initialize frightening events
            frighteningEvents = roomTransform.GetComponentsInChildren<FrighteningEventBase>();
            foreach (var frighteningEventBase in frighteningEvents)
            {
                if (frighteningEventBase == null)
                    continue;
                frighteningEventBase.CurrentRoom = this;
            }
            
            // Find garlics
            garlics = new List<Garlic>(roomTransform.GetComponentsInChildren<Garlic>());
            
            // Initialize colliders
            if (!roomTransform)
            {
                Debug.LogWarning($"Room colliders tranform of room named '{name}' is not given");
                return;
            }
            roomColliders = roomTransform.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D roomCollider in roomColliders)
                roomCollider.isTrigger = true;
        }

        private void UpdateLightState()
        {
            if (curtains.Length == 0)
            {
                lighten = false;
                return;
            }

            foreach (var curtain in curtains)
            {
                if (curtain == null)
                    continue;
                
                if (!curtain.IsOpened)
                {
                    lighten = false;
                    return;
                }
            }

            lighten = true;
        }

        /// <summary>
        /// Give the closest enemy from a given position in this room
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public EnemyBase GetClosestEnemy(Vector2 worldPosition)
        {
            if (enemies.Count == 0)
                return null;
            
            GridManager gridManager = GridManager.Instance;

            int closestDistanceIndex = 0;
            int closestDistance = gridManager.GetManhattanDistance(worldPosition, enemies[0].transform.position);
            for (int i = 1; i < enemies.Count; i++)
            {
                int distance = gridManager.GetManhattanDistance(worldPosition, enemies[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDistanceIndex = i;
                }
            }

            return enemies[closestDistanceIndex];
        }

        /// <summary>
        /// Tell if a collider is the collider of the room
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public bool IsRoomCollider(Collider2D collider)
        {
            if (!collider.isTrigger || (roomColliders == null || roomColliders.Length == 0))
                return false;

            foreach (var roomCollider in roomColliders)
            {
                if (roomCollider == collider)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Tell if a given position is inside the range of any garlic in the room
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="damagingGarlic"></param>
        /// <returns></returns>
        public bool IsInGarlicRange(Vector2 worldPosition, out Garlic damagingGarlic)
        {
            foreach (Garlic garlic in garlics)
            {
                if (garlic == null)
                    continue;
                
                if (garlic.IsInRange(worldPosition))
                {
                    damagingGarlic = garlic;
                    return true;
                }
            }

            damagingGarlic = null;
            return false;
        }

        /// <summary>
        /// Tell if a given position is inside the light of any window of the room
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public bool IsInLight(Vector2 worldPosition)
        {
            if (GameManager.dayState != GameManager.DayState.DAY)
                return false;

            if (lighten)
                return true;

            foreach (var curtain in curtains)
            {
                if (curtain == null)
                    continue;
                
                if (!curtain.IsOpened)
                    continue;

                if (curtain.IsInsideLight(worldPosition))
                    return true;
            }

            return false;
        }
    }
}