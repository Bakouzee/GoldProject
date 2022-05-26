using System.Collections.Generic;
using GoldProject.Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GridSystem
{
    public class GridManager : SingletonBase<GridManager>
    {
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Tilemap tilemap;

        // Grid size is defined by taking the mapSize from the RoomsManager in Awake/Start
        private Vector2Int gridSize;
        private Tile[,] tiles;
        private Transform tilesParent;

        #region Get Tile at Position

        public Tile GetTileAtPosition(Vector3 worldPosition) =>
            GetTileAtPosition(new Vector2Int((int) worldPosition.x, (int) worldPosition.y));

        public Tile GetTileAtPosition(Vector2Int gridPos) => tiles[gridPos.x, gridPos.y];

        #endregion

        public bool IsInGrid(Vector2Int gridPos) =>
            0 <= gridPos.x && gridPos.x < gridSize.x && 0 <= gridPos.y && gridPos.y < gridSize.y;

        public bool HasTile(Vector2Int gridPos)
        {
            if (!IsInGrid(gridPos))
                return false;
            return tiles[gridPos.x, gridPos.y] != null;
        }

        [SerializeField] private bool debug;

        protected override void Awake()
        {
            base.Awake();
            GenerateGrid();
        }

        void GenerateGrid()
        {
            Vector2 mapSize = RoomsManager.Instance.mapSize;
            gridSize = new Vector2Int(Mathf.CeilToInt(mapSize.x), Mathf.CeilToInt(mapSize.y));

            tilesParent = new GameObject("Tiles").transform;
            tilesParent.SetParent(transform);

            tiles = new Tile[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        Tile newTile = Instantiate(
                            tilePrefab, 
                        new Vector3(x + 0.5f, y + 0.5f, 0f),
                            Quaternion.identity
                        );
                        newTile.SetGridPos(new Vector2Int(x, y));
                        tiles[x, y] = newTile;
                    }
                }
            }

            tilemap.gameObject.SetActive(false);
        }

        public List<Direction> GetPath(Vector2Int startGridPos, Vector2Int aimedGridPos)
        {
            if (startGridPos == aimedGridPos)
            {
                Debug.Log("chemin généré a 100%");
                return null;
            }

            List<Direction> path = new List<Direction>();

            // Get tiles
            Tile origin = GetTileAtPosition(startGridPos);
            Tile end = GetTileAtPosition(aimedGridPos);

            // Set startTile green
            if (debug)
                origin.GetComponent<SpriteRenderer>().color = Color.green;

            // Get the best tile
            Tile smallestTile = null;
            foreach (Tile tile in GetTouchingTiles(origin))
            {
                if (tile != null)
                {
                    tile.GCost = GetManhattanDistance(origin, tile);
                    tile.HCost = GetManhattanDistance(tile, end);


                    if (smallestTile == null || smallestTile.FCost > tile.FCost)
                        smallestTile = tile;
                    else if (smallestTile.FCost == tile.FCost)
                        smallestTile = smallestTile.HCost > tile.HCost ? tile : smallestTile;
                }
            }

            // Get direction to the best tile
            Vector2Int gridDirection = smallestTile.GridPos - origin.GridPos;
            Direction dir = Direction.FromVector2Int(gridDirection);

            // Add to path
            // path.Add(smallestTile, dir);
            path.Add(dir);

            if (debug)
                smallestTile.gameObject.GetComponent<SpriteRenderer>().color =
                    new Color(Color.green.r, Color.green.g, Color.green.b, 0.3f);


            // If already reached target
            if ((Vector2) smallestTile.GridPos == aimedGridPos)
                return path;

            path.AddRange(GetPath(smallestTile.GridPos, aimedGridPos));
            return path;
        }

        #region Get Manhattan Distance

        public int GetManhattanDistance(Vector2Int a, Vector2Int b) =>
            Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        public int GetManhattanDistance(Tile a, Tile b) =>
            GetManhattanDistance(a.GridPos, b.GridPos);

        #endregion

        private Tile[] GetTouchingTiles(Tile tile)
        {
            Tile[] nearTiles = new Tile[4];

            Vector2Int tileOriginCoords = tile.GridPos;

            nearTiles[0] = GetTileAtPosition(new Vector2Int(tileOriginCoords.x, tileOriginCoords.y - 1));
            nearTiles[1] = GetTileAtPosition(new Vector2Int(tileOriginCoords.x - 1, tileOriginCoords.y));
            nearTiles[2] = GetTileAtPosition(new Vector2Int(tileOriginCoords.x + 1, tileOriginCoords.y));
            nearTiles[3] = GetTileAtPosition(new Vector2Int(tileOriginCoords.x, tileOriginCoords.y + 1));

            return nearTiles;
        }
    }
}