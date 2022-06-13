using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GoldProject.Rooms
{
    public class RoomsManager : SingletonBase<RoomsManager>
    {
        public Vector2 mapSize;
        public Vector2 playerFovSize;
        [SerializeField] private Room[] rooms;
        
        #region Rooms getter/setter
        public Room[] Rooms => rooms;
        public Room GetRandomRoom()
        {
            var roomsLength = rooms.Length;
            Room room = GetRoom(UnityEngine.Random.Range(0, roomsLength));
            float random = UnityEngine.Random.value;
            if (random <= 0.75f)
            {
                // Only take non corridor room
                while (room == null || room.isCorridor)
                {
                    room = GetRoom(UnityEngine.Random.Range(0, roomsLength));
                }
            }
            else
            {
                // Only take corridor room
                while(room == null || !room.isCorridor)
                    room = GetRoom(UnityEngine.Random.Range(0, roomsLength));
            }
            return room;
        }
        public Room GetRoom(int index) => rooms[index % rooms.Length];
        #endregion

        private void Start()
        {
            foreach (var room in rooms)
            {
                room.Initialize();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(mapSize * 0.5f, mapSize);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            foreach (var room in rooms)
            {
                if (room == null || room.roomTransform == null)
                    continue;
                Gizmos.DrawWireCube(room.Position,(Vector2)(room.size));
            }
        }

        public Room GetColliderRoom(Collider2D collider2D) => rooms.FirstOrDefault(room => room.IsRoomCollider(collider2D));
        public bool IsCorridor(Room room) => room.isCorridor;
    }
}