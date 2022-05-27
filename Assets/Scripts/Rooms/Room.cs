using System.Collections.Generic;
using GoldProject.FrighteningEvent;
using UnityEngine;

namespace GoldProject.Rooms
{
    [System.Serializable]
    public class Room
    {
        public string name;
        public bool isCorridor;
        public Vector2 position;
        public Vector2Int size;

        private bool lighten;
        public bool IsLighten => lighten;

        [Header("Objects")] public Curtain[] curtains;
        [SerializeReference] public FrighteningEventBase[] frighteningEvents;
        public List<Garlic> garlics;

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

                curtain.SetOpened(true);
                curtain.onStateChanged = UpdateLightState;
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

        public bool IsRoomCollider(Collider2D collider)
        {
            if ( !collider.isTrigger || (roomColliders == null || roomColliders.Length == 0))
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
    }
}