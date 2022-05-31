﻿using System.Collections.Generic;
using Enemies;
using GoldProject.FrighteningEvent;
using GridSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GoldProject.Rooms
{
    [System.Serializable]
    public class Room
    {
        public string name;
        public bool isCorridor;
        public Vector2 position;
        public Vector2Int size;

        public bool IsInside(Vector2 worldPosition)
        {
            float halfLength = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;
            return position.x - halfLength < worldPosition.x &&
                   worldPosition.x < position.x + halfLength && 
                   position.y - halfHeight < worldPosition.y &&
                   worldPosition.y < position.y + halfHeight;
        }

        private bool lighten;
        public bool IsLighten => lighten;

        [Header("Objects")] public Curtain[] curtains;
        [SerializeReference] public FrighteningEventBase[] frighteningEvents;
        [Attributes.ReadOnly] public List<Garlic> garlics;
        [Attributes.ReadOnly] public List<Enemies.EnemyBase> enemies = new List<EnemyBase>();
        public Transform[] pathPoints;

        [Header("Colliders")] [Tooltip("GameObject contaning all the colliders of the room")]
        public Transform roomCollidersTransform;

        private Collider2D[] roomColliders;

        public void Initialize()
        {
            // Initialize curtains
            foreach (Curtain curtain in curtains)
            {
                if (curtain == null)
                    continue;
                curtain.SetOpened(false);
                curtain.onStateChanged = UpdateLightState;
            }
            
            // Initialize frightening events
            foreach (var frighteningEventBase in frighteningEvents)
            {
                frighteningEventBase.CurrentRoom = this;
            }

            // Initialize colliders
            if (!roomCollidersTransform)
            {
                Debug.LogWarning($"Room colliders tranform of room named '{name}' is not given");
                return;
            }

            roomColliders = roomCollidersTransform.GetComponentsInChildren<Collider2D>();
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
                if (!curtain.IsOpened)
                {
                    lighten = false;
                    return;
                }
            }

            lighten = true;
        }

        public EnemyBase GetClosestEnemy(Vector2 worldPosition)
        {
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

        public bool IsInGarlicRange(Vector2 worldPosition, out Garlic damagingGarlic)
        {
            foreach (Garlic garlic in garlics)
            {
                if (garlic.IsInRange(worldPosition))
                {
                    damagingGarlic = garlic;
                    return true;
                }
            }

            damagingGarlic = null;
            return false;
        }

        public bool IsInLight(Vector2 worldPosition)
        {
            if (lighten)
                return true;

            foreach (var curtain in curtains)
            {
                
            }

            return false;
        }
    }
}