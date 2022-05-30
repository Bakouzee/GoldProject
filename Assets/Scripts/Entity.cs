﻿using System;
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

        protected override void Start()
        {
            base.Start();
            
            // Find current room
            foreach (var room in RoomsManager.Instance.Rooms)
            {
                if (room.IsInside(transform.position))
                {
                    currentRoom = room;
                    break;
                }
            }
        }

        private Collider2D lastCollider2D;
        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (lastCollider2D != other)
            {
                Room newRoom = RoomsManager.Instance.GetColliderRoom(other);
                if (newRoom != null)
                {
                    if(currentRoom != null) OnExitRoom(currentRoom);
                    currentRoom = newRoom;
                    OnEnterRoom(currentRoom);
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