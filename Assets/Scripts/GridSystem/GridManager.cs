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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && testAI)
                GetPath(testStart,testAimed);           
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

            Debug.Log("direct path");

            testAI = true;
            testStart = startPos;
            testAimed = aimedPos;

            // Il faut que a chaque fois on génére les 4 tiles dans chacune direction
            // Puis calcule du H et du G

            Tile origin = GetTileFromObjectPosition(startPos);
            Tile end = GetTileFromObjectPosition(aimedPos);

            Debug.Log("length: " + FindTilesNear(origin).Length);
            
            foreach(Tile tile in FindTilesNear(origin)) {
                Debug.Log("tile: " + tile);
                if(tile != null) {
                    tile.GCost = GetDistance(origin, tile);
                    tile.HCost = GetDistance(tile, end);

                    Debug.Log("GCost: " + tile.GCost);
                    Debug.Log("HCost: " + tile.HCost);
                    Debug.Log("FCost: " + tile.FCost);
                }
            }

            return path;
        }

        private int GetDistance(Tile first,Tile second) {

            Vector2Int firstCoord = FindTileCoords(first);
            Vector2Int secondCoord = FindTileCoords(second);
            
            return Mathf.Abs((secondCoord.x - firstCoord.x) + (secondCoord.y - firstCoord.y));
        }

        private Tile[] FindTilesNear(Tile origin) {
            Tile[] nearTiles = new Tile[4];

            Vector2Int tileOriginCoords = FindTileCoords(origin);

            nearTiles[0] = FindTileWithTileCoords(new int[] { tileOriginCoords.x,tileOriginCoords.y + 1});
            nearTiles[1] = FindTileWithTileCoords(new int[] { tileOriginCoords.x - 1, tileOriginCoords.y });
            nearTiles[2] = FindTileWithTileCoords(new int[] { tileOriginCoords.x + 1, tileOriginCoords.y });
            nearTiles[3] = FindTileWithTileCoords(new int[] { tileOriginCoords.x, tileOriginCoords.y - 1 });

            return nearTiles;

        }

        private Vector2Int FindTileCoords(Tile tile)  {
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
                if(FindTileCoords(tile)[0] == coords[0] && FindTileCoords(tile)[1] == 1)          
                    return tile;   
            }

            return null;
        }

        // Permet de trouver la tile a la position donner 
        private Tile GetTileFromObjectPosition(Vector3 objPosition) { // Position = coordonnées joueurs 
            Tile tile = null;

            foreach (Tile currentTile in tiles) {

                if(currentTile != null && currentTile.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer)) {
                    Vector3[] corners = { renderer.transform.TransformPoint(renderer.sprite.bounds.max), // topRight 
                        renderer.transform.TransformPoint(new Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0)),// topLeft
                        renderer.transform.TransformPoint(renderer.sprite.bounds.min), // bottomLeft
                    };

                    if ((int)objPosition.x >= (int)corners[1].x && (int)objPosition.x <= (int)corners[0].x)  {
                        if ((int)objPosition.y >= corners[0].y && (int)objPosition.y <= corners[2].y)  {
                            tile = currentTile;
                            break;
                        }
                    }
                }
            }

            return tile;
        }
    }
}