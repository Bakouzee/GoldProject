using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace GridSystem
{
    public class GridManager : SingletonBase<GridManager>
    {
        [SerializeField] private Vector2Int levelDimensions;
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Tilemap tilemap;
        private Tile[,] tiles;

        private void Start()
        {
            Camera camera = Camera.main;
            Vector2 centerPos = levelDimensions;
            centerPos *= 0.5f;

            camera.transform.position = new Vector3(centerPos.x, centerPos.y, camera.transform.position.z);
            camera.orthographicSize = levelDimensions.x * 0.28f;
            
            GenerateGrid();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2 centerPos = levelDimensions;
            centerPos *= 0.5f;
            Vector2 cubeSize = levelDimensions;
            Gizmos.DrawWireCube(centerPos, cubeSize);
        }

        void GenerateGrid()
        {
            tiles = new Tile[levelDimensions.x, levelDimensions.y];
            for (int x = 0; x < levelDimensions.x; x++)
            {
                for (int y = 0; y < levelDimensions.y; y++)
                {
                    if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        Tile newTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                        tiles[x, y] = newTile;
                    }
                }
            }
            tilemap.gameObject.SetActive(false);
        }

        public Stack<Direction> GetPath(Vector2 startPos, Vector2 aimedPos)
        {
            Stack<Direction> path = new Stack<Direction>();

            
            
            return path;
        }
    }
}