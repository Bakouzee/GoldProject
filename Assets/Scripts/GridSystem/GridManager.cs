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
        private Vector2Int gridSize;
        public Tile[,] tiles;
        public bool IsInGrid(Vector2Int gridPos) =>
            0 <= gridPos.x && gridPos.x < gridSize.x && 0 <= gridPos.y && gridPos.y < gridSize.y;
        public bool HasTile(Vector2Int gridPos)
        {
            if (!IsInGrid(gridPos))
                return false;
            return tiles[gridPos.x, gridPos.y] != null;
        }

        private bool testAI;
        private Vector2 testStart, testAimed;

        protected override void Awake()
        {
            base.Awake();
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
            Vector2 mapSize = RoomsManager.Instance.mapSize;
            gridSize = new Vector2Int(Mathf.CeilToInt(mapSize.x), Mathf.CeilToInt(mapSize.y));
            
            tiles = new Tile[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        Tile newTile = Instantiate(tilePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity, transform);
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
                return path;
            }



            testStart = smallestTile.transform.position;

            

            return GetPath(testStart,testAimed);
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