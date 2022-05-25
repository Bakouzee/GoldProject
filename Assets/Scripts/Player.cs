using System;
using GoldProject.Rooms;
using Unity.VisualScripting;
using UnityEngine;

namespace GoldProject
{
    public class Player : Entity
    {
        private void Start()
        {
            currentRoom = RoomsManager.Instance.FirstRoom;
        }
    }
}