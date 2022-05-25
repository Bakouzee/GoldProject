using GoldProject.Rooms;
using GridSystem;
using UnityEngine;

namespace GoldProject
{
    public class Player : Entity
    {
        private CameraController cameraController;
        [SerializeField] private Vector2Int spawnGridPos;

        protected override void Start()
        {
            base.Start();
            cameraController = FindObjectOfType<CameraController>();
            
            // Set current room
            currentRoom = RoomsManager.Instance.FirstRoom;
            OnEnterRoom();
            
            // Set position on grid
            SetPosition(spawnGridPos);
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if (horizontal != 0)
                {
                    if(Move(horizontal > 0 ? Direction.Right : Direction.Left))
                        GameManager.Instance.MoveAllEnemies();
                }
                else if (vertical != 0)
                {
                    if(Move(vertical > 0 ? Direction.Up : Direction.Down))
                        GameManager.Instance.MoveAllEnemies();
                }
            }
        }

        protected override void OnEnterRoom()
        {
            cameraController.ZoomToRoom(currentRoom);
        }
    }
}