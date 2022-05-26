using System.Collections;
using System.Collections.Generic;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GoldProject
{
    public class Player : Entity
    {
        private CameraController cameraController;

        [SerializeField] private float moveCooldown;
        private bool hasPath;
        private List<Direction> path = new List<Direction>();
        
        protected override void Start()
        {
            base.Start();
            cameraController = FindObjectOfType<CameraController>();

            // Set current room
            currentRoom = RoomsManager.Instance.FirstRoom;
            OnEnterRoom();

            // Set position on grid
            var position = transform.position;
            Vector2Int spawnGridPos = new Vector2Int((int) position.x, (int) position.y);
            SetPosition(spawnGridPos);
        }

        private void Update()
        {
            if (hasPath)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.right, 0.1f);

                if (!hit)
                    return;
                if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                {
                    StartPath(tile.GridPos);
                }
            }
        }

        private void StartPath(Vector2Int aimedGridPos)
        {
            if (hasPath || moveCoroutine != null)
                return;
            path = gridManager.GetPath(gridPosition, aimedGridPos);
            hasPath = true;
            
            moveCoroutine = MoveCoroutine();
            StartCoroutine(moveCoroutine);
        }

        private IEnumerator moveCoroutine;
        IEnumerator MoveCoroutine()
        {
            foreach (Direction direction in path)
            {
                Move(direction);
                GameManager.Instance.MoveAllEnemies();
                yield return new WaitForSeconds(moveCooldown);
            }

            hasPath = false;
            moveCoroutine = null;
        }

        protected override void OnEnterRoom()
        {
            cameraController.ZoomToRoom(currentRoom);
        }
    }
}