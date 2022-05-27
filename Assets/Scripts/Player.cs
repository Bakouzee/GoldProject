﻿using System.Collections;
using System.Collections.Generic;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;

namespace GoldProject
{
    public class Player : Entity
    {
        public PlayerManager PlayerManager { private get; set; }
        private CameraController cameraController;

        [SerializeField] private float moveCooldown;
        private bool hasPath;
        private List<Direction> path = new List<Direction>();
        
        protected override void Start()
        {
            base.Start();
            cameraController = FindObjectOfType<CameraController>();
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

        protected override void OnMoved()
        {
            if (currentRoom.IsInGarlicRange(transform.position, out Garlic damagingGarlic))
            {
                Debug.Log("Garlic in range");
                PlayerManager.PlayerHealth.TakeDamage(damagingGarlic.damage);
            }
        }

        protected override void OnEnterRoom(Room room)
        {
            cameraController.ZoomToRoom(room);
        }
    }
}