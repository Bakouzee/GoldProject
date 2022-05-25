﻿using System;
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

        private bool testAI;
        private Vector2 testStart, testAimed;


        private void Start()
        {
            Camera camera = Camera.main;
            Vector2 centerPos = levelDimensions;
            centerPos *= 0.5f;

            camera.transform.position = new Vector3(centerPos.x, centerPos.y, camera.transform.position.z);
            camera.orthographicSize = levelDimensions.x * 0.28f;
            
            GenerateGrid();
        }

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Space) && testAI) 
                GetPath(testStart, testAimed);
            
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
                        newTile.name = "{" + x + ";" + y + "}";
                    }
                }
            }
            tilemap.gameObject.SetActive(false);
        }

        public Stack<Direction> GetPath(Vector2 startPos, Vector2 aimedPos)
        {
            Stack<Direction> path = new Stack<Direction>();

            testAI = true;
            testStart = startPos;
            testAimed = aimedPos;

            Tile origin = GetTileFromObjectPosition(testStart);
            Tile end = GetTileFromObjectPosition(testAimed);

            origin.GetComponent<SpriteRenderer>().color = Color.green;

            Tile smallestTile = null;


            foreach(Tile tile in FindTilesNear(origin)) {
               if (tile != null) {
                   tile.GCost = GetDistance(origin, tile);
                   tile.HCost = GetDistance(tile, end);

                   tile.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                   if(smallestTile == null || smallestTile.FCost > tile.FCost) 
                       smallestTile = tile;          
                   else if(smallestTile.FCost == tile.FCost) 
                       smallestTile = smallestTile.HCost > tile.HCost ? tile : smallestTile;
                    
               }
            }

            if(testStart == testAimed) {
                Debug.Log("chemin généré a 100%");
                testAI = false;
            }

            testStart = smallestTile.transform.position;

            

            return path;
        }

        private int GetDistance(Tile first,Tile second) {

            Vector2Int firstCoord = GetTileCoords(first);
            Vector2Int secondCoord = GetTileCoords(second);
            
            return Mathf.Abs(secondCoord.x - firstCoord.x) + Mathf.Abs(secondCoord.y - firstCoord.y);
        }

        private Tile[] FindTilesNear(Tile origin) {
            Tile[] nearTiles = new Tile[4];

            Vector2Int tileOriginCoords = GetTileCoords(origin);

           // nearTiles[0] = FindTileWithTileCoords(new int[] { tileOriginCoords.x,tileOriginCoords.y - 1});
            nearTiles[1] = FindTileWithTileCoords(new int[] { tileOriginCoords.x - 1, tileOriginCoords.y });
            nearTiles[2] = FindTileWithTileCoords(new int[] { tileOriginCoords.x + 1, tileOriginCoords.y });
            nearTiles[3] = FindTileWithTileCoords(new int[] { tileOriginCoords.x, tileOriginCoords.y + 1 });

            return nearTiles;

        }

        public Vector2Int GetTileCoords(Tile tile)  {
            Vector2Int tileCoords = new Vector2Int();

            for(int x = 0; x < tiles.GetLength(0);x++) {
                for(int y = 0; y < tiles.GetLength(1);y++) {
                    if(tiles[x,y] == tile) {
                        tileCoords.x = x;
                        tileCoords.y = y;
                    }
                
                }
            }

            return tileCoords;  
        }

        private Tile FindTileWithTileCoords(int[] coords) {
            foreach(Tile tile in tiles) {
                if(GetTileCoords(tile)[0] == coords[0] && GetTileCoords(tile)[1] == coords[1])          
                    return tile;   
            }

            return null;
        }

        // Permet de trouver la tile a la position donner 
        private Tile GetTileFromObjectPosition(Vector3 objPosition) { // Position = coordonnées joueurs 
            Tile tile = null;   

            RaycastHit2D hit = Physics2D.Raycast(objPosition,Vector3.right,1f);
            if (hit.collider != null)  
                tile = hit.collider.gameObject.GetComponent<Tile>();

            return tile;
        }
    }
}