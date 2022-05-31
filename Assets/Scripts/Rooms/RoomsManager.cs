using System;
using System.Linq;
using UnityEngine;

namespace GoldProject.Rooms
{
    public class RoomsManager : SingletonBase<RoomsManager>
    {
        public Vector2 mapSize;
        [SerializeField] private Room[] rooms;
        #region Rooms getter/setter
        public Room[] Rooms => rooms;
        public Room GetRandomRoom() => GetRoom(UnityEngine.Random.Range(0, rooms.Length));
        public Room GetRoom(int index) => rooms[index % rooms.Length];
        [HideInInspector] public Room FirstRoom;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            FirstRoom = rooms[0];
        }

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
                Gizmos.DrawWireCube(room.position,(Vector2)(room.size));
        }

        public Room GetColliderRoom(Collider2D collider2D) => rooms.FirstOrDefault(room => room.IsRoomCollider(collider2D));
        public bool IsCorridor(Room room) => room.isCorridor;
    }
}