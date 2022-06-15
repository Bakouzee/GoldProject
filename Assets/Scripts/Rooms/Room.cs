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
        public Vector2 Position => (Vector2)roomTransform.position + PrimaryCollider.offset;
        private BoxCollider2D primaryCollider;
        private BoxCollider2D PrimaryCollider
        {
            get
            {
                if (primaryCollider == null)
                    primaryCollider = roomTransform.GetComponent<BoxCollider2D>();
                return primaryCollider;
            }
        }
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
        public bool IsLighten
        {
            get => lighten;
            private set
            {
                lighten = value;
                if (fullRoomLight)
                    fullRoomLight.gameObject.SetActive(lighten);
            }
        }
        
        public Light2D fullRoomLight;
        
        [HideInInspector] public Curtain[] curtains;
        [HideInInspector] public FrighteningEventBase[] frighteningEvents;
        [HideInInspector] public VentManager[] vents;
        [HideInInspector] public List<Garlic> garlics;
        /*[HideInInspector]*/ public List<Enemies.EnemyBase> enemies = new List<EnemyBase>();
        [Space(10)]
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
            
            
            // Find vents
            vents = roomTransform.GetComponentsInChildren<VentManager>();
            // Find garlics
            garlics = new List<Garlic>(roomTransform.GetComponentsInChildren<Garlic>());
            GameManager.Instance.OnLaunchedTurn += (turnCount, turnPerPhase) =>
            {
                foreach (var garlic in garlics.ToArray())
                {
                    if (garlic.DecrementLifeTime())
                    {
                        garlics.Remove(garlic);
                    }
                }
            };
            
            // Initialize colliders
            if (!roomTransform)
            {
                Debug.LogWarning($"Room colliders tranform of room named '{name}' is not given");
                return;
            }
            roomColliders = roomTransform.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D roomCollider in roomColliders)
                roomCollider.isTrigger = true;

            // Full room light
            if (fullRoomLight)
                fullRoomLight.gameObject.SetActive(false);
        }

        private void UpdateLightState()
        {
            if (curtains.Length == 0)
            {
                IsLighten = false;
                return;
            }

            foreach (var curtain in curtains)
            {
                if (curtain == null)
                    continue;
                
                if (!curtain.IsOpened)
                {
                    IsLighten = false;
                    return;
                }
            }

            IsLighten = true;
        }

        #region Get Closest T

        /// <summary>Give the closest curtain from a given position in this room</summary>
        public Curtain GetClosestCurtain(Vector2 worldPosition) => 
            GetClosest<Curtain>(worldPosition, in curtains);
        
        /// <summary>Give the closest enemy from a given position in this room</summary>
        public EnemyBase GetClosestEnemy(Vector2 worldPosition)
        {
            EnemyBase[] enemiesArray = this.enemies.ToArray();
            return GetClosest<EnemyBase>(worldPosition, in enemiesArray);
        }
        
        /// <summary>Give the closest vent from a given position in this room</summary>
        public VentManager GetClosestVent(Vector2 worldPosition) => 
            GetClosest(worldPosition, in vents);

        /// <summary>Give the closest FrighteningEvent from a given position in this room</summary>
        public FrighteningEventBase GetClosestFrighteningEvent(Vector2 worldPosition) =>
            GetClosest(worldPosition, in frighteningEvents);
        
        private T GetClosest<T>(Vector2 worldPosition, in T[] array) where T : MonoBehaviour
        {
            if (array.Length == 0)
                return null;

            GridManager gridManager = GridManager.Instance;
            
            int closestDistanceIndex = 0;
            int closestDistance = gridManager.GetManhattanDistance(worldPosition, array[0].transform.position);
            for (int i = 1; i < array.Length; i++)
            {
                int distance = gridManager.GetManhattanDistance(worldPosition, array[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDistanceIndex = i;
                }
            }
            return array[closestDistanceIndex];
        }
        #endregion

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

            if (IsLighten)
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