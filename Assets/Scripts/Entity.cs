using System;
using GoldProject;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;

namespace GoldProject
{
    public class Entity : GridController, ILocalizable
    {
        protected Room currentRoom;
        public Room CurrentRoom => currentRoom;

        private Collider2D lastCollider2D;
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (lastCollider2D != other)
            {
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
}