using System;
using System.Collections.Generic;
using GoldProject.Rooms;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

namespace GridSystem
{
    public class GridManager : SingletonBase<GridManager>
    {
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Tilemap tilemap;
        private Tile[,] tiles;

        private void Start()
        {
            GenerateGrid();
        }

        void GenerateGrid()
        {
            Vector2 mapSize = RoomsManager.Instance.mapSize;
            Vector2Int levelDimensions = new Vector2Int( Mathf.CeilToInt(mapSize.x), Mathf.CeilToInt(mapSize.y));
            
            tiles = new Tile[levelDimensions.x, levelDimensions.y];
            for (int x = 0; x < levelDimensions.x; x++)
            {
                for (int y = 0; y < levelDimensions.y; y++)
                {
                    if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        Tile newTile = Instantiate(tilePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity, transform);
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