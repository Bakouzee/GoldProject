using GoldProject.Rooms;
using GridSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GoldProject
{
    public class Player : Entity
    {
        private CameraController cameraController;
        [SerializeField] private Vector2Int spawnGridPos;
        private Dictionary<Tile,Direction> path;
        private Tile actualTile;

        private Direction targetDirection;

        private GridManager grid;

        private int currentIndex;

        protected override void Start()
        {
            base.Start();
            cameraController = FindObjectOfType<CameraController>();
            
            // Set current room
            currentRoom = RoomsManager.Instance.FirstRoom;
            OnEnterRoom();
            
            // Set position on grid
            SetPosition(spawnGridPos);

            path = new Dictionary<Tile,Direction>();
            grid = GridManager.Instance;
        }

        private void Update()
        {

            if(Input.GetMouseButtonDown(0)) {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePosition,Vector3.right,0.1f);

                if(hit.collider != null && hit.collider.gameObject.GetComponent<Tile>() != null) {                  
                    path = grid.GetPath(transform.position,hit.collider.gameObject.transform.position);
                    actualTile = grid.GetTileFromObjectPosition(transform.position);
                }
            }

            if (path != null && path.Count > 0) {

                int tileIndex = path.Keys.ToList().IndexOf(actualTile);

                if(tileIndex + 1 >= path.Count)  {
                    path.Clear();
                    transform.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    return;
                }

                Direction nextDirection = path.Values.ToList()[tileIndex + 1];
                Move(nextDirection.value);

            }
            


        }

        protected override void OnEnterRoom()
        {
            cameraController.ZoomToRoom(currentRoom);
        }

        private void OnTriggerStay2D(Collider2D collision) {
            if (collision.gameObject.TryGetComponent<Tile>(out Tile tile))
            {
                Vector3 tilePos = collision.gameObject.transform.position;
                Vector3 playerPos = transform.position;

                float distance = Vector3.Distance(tilePos, playerPos);

                if( distance < 0.1f)
                
                    actualTile = tile;        
            }
        }
    }
}