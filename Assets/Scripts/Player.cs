using System;
using GoldProject.Rooms;
using Unity.VisualScripting;
using UnityEngine;

namespace GoldProject
{
    public class Player : MonoBehaviour, ILocalizable
    {
        private Room currentRoom;
        public Room CurrentRoom => currentRoom;

        private void Start()
        {
            currentRoom = RoomsManager.Instance.FirstRoom;
        }
        
        private Collider2D lastCollider2D = null;
        private void OnTriggerStay2D(Collider2D other)
        {
            if (lastCollider2D == other)
                return;

            Room newRoom = RoomsManager.Instance.GetColliderRoom(other);
            if (newRoom != null)
            {
                currentRoom = newRoom;
                Debug.Log(newRoom.name);
            }

            lastCollider2D = other;
        }
    }
}