using System.Linq;
using UnityEngine;

namespace GoldProject.Rooms
{
    public class RoomsManager : SingletonBase<RoomsManager>
    {
        [SerializeField] private Room[] rooms;

        private void Start()
        {
            foreach (var room in rooms)
            {
                room.Initialize();
            }
        }

        public Room GetColliderRoom(Collider2D collider2D) => rooms.FirstOrDefault(room => room.IsRoomCollider(collider2D));
    }
}