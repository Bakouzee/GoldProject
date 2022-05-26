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

        public Vector2Int gridSize; // A remttre en privé
        public Tile[,] tiles;
        public bool IsInGrid(Vector2Int gridPos) =>
            0 <= gridPos.x && gridPos.x < gridSize.x && 0 <= gridPos.y && gridPos.y < gridSize.y;

        public bool HasTile(Vector2Int gridPos)
        {

           
            if (!IsInGrid(gridPos))
                return false;
            return tiles[gridPos.x, gridPos.y] != null;
        }

        private Vector2 originPos, targetPos;


        public Dictionary<Tile,Direction> path;

        [SerializeField]
        private bool debug;

        protected override void Awake()
        {
            base.Awake();
            GenerateGrid();

            path = new Dictionary<Tile,Direction>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2 centerPos = gridSize;
            centerPos *= 0.5f;
            Vector2 cubeSize = gridSize;
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
                        Tile newTile = Instantiate(tilePrefab, new Vector3(x + 0.5f, y + 0.5f, 0f), Quaternion.identity, transform);
                        tiles[x, y] = newTile;
                        newTile.name = "{" + x + ";" + y + "}";
                        
                        if (debug)  {
                            newTile.gameObject.transform.localScale = new Vector3(2f, 2f, 0f);
                            newTile.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                    }
                }
            }
            tilemap.gameObject.SetActive(false);
        }

        public Dictionary<Tile,Direction> GetPath(Vector2 startPos, Vector2 aimedPos) {
            
            originPos = startPos;
            targetPos = aimedPos;

            Tile origin = GetTileFromObjectPosition(originPos);
            Tile end = GetTileFromObjectPosition(targetPos);

            if(debug)
                origin.GetComponent<SpriteRenderer>().color = Color.green;

            Tile smallestTile = null;

            foreach (Tile tile in FindTilesNear(origin)) {
                if (tile != null) {
                   tile.GCost = GetDistance(origin, tile);
                   tile.HCost = GetDistance(tile, end);


                    if (smallestTile == null || smallestTile.FCost > tile.FCost) 
                       smallestTile = tile;          
                   else if(smallestTile.FCost == tile.FCost) 
                       smallestTile = smallestTile.HCost > tile.HCost ? tile : smallestTile;
                    
               }
            }

            


            if (originPos == targetPos) {
                Debug.Log("chemin généré a 100%");
                return path;
            }

            Direction dir = new Direction(ConvertDirection(origin, smallestTile));
            path.Add(smallestTile,dir);


            smallestTile.gameObject.GetComponent<SpriteRenderer>().color = new Color(Color.green.r, Color.green.g, Color.green.b,0.3f);

            originPos = smallestTile.transform.position;

            return GetPath(originPos,targetPos);
        }

        private string ConvertDirection(Tile last,Tile actual) {
            Vector2Int lastCoord = GetTileCoords(last);
            Vector2Int actualCoord = GetTileCoords(actual);

            int diffX = actualCoord.x - lastCoord.x;
            int diffY = actualCoord.y - lastCoord.y;

            if (diffX == 1)
                return "Right";
            else if (diffX == -1)
                return "Left";

            if (diffY == 1)
                return "Up";
            else if (diffY == -1)
                return "Down";     

            return "";
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
        public Tile GetTileFromObjectPosition(Vector3 objPosition) { // Position = coordonnées joueurs 
            Tile tile = null;   

            RaycastHit2D hit = Physics2D.Raycast(objPosition,Vector3.right,1f);

            if (hit.collider != null)  
                tile = hit.collider.gameObject.GetComponent<Tile>();

            return tile;
        }

   
    }
}