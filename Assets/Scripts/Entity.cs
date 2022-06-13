using System;
using GoldProject;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;

namespace GoldProject
{
    public class Entity : MonoBehaviour, ILocalizable
    {
        public GridController gridController;
        protected Room currentRoom;
        public Room CurrentRoom
        {
            get
            {
                return currentRoom;
            }
            protected set
            {
                if(currentRoom != null) OnExitRoom(currentRoom);
                currentRoom = value;
                OnEnterRoom(currentRoom);
            }
        }

        protected virtual void Start()
        {
            gridController = new GridController(transform);
         
            UpdateCurrentRoom();
        }

        protected virtual void UpdateCurrentRoom()
        {
            // Find current room
            foreach (var room in RoomsManager.Instance.Rooms)
            {
                if (room.IsInside(transform.position))
                {
                    CurrentRoom = room;
                    break;
                }
            }
        }

        private Collider2D lastCollider2D;
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (lastCollider2D != other && other.tag != "Vent")
            {
                Room newRoom = RoomsManager.Instance.GetColliderRoom(other);
                if (newRoom != null)
                {
                    CurrentRoom = newRoom;
                }

                lastCollider2D = other;
            }
        }

        protected virtual void OnExitRoom(Room room)
        {
            
        }
        protected virtual void OnEnterRoom(Room room)
        {
            
        }
    }
}