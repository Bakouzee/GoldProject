using System.Collections.Generic;
using System.Linq;
using GoldProject.Rooms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
                            new Vector3(x + 0.5f, y + 0.5f, 5f),
                            Quaternion.identity,
                            tilesParent
                        );
                        newTile.SetGridPos(new Vector2Int(x, y));
                        tiles[x, y] = newTile;
                    }
                }
            }

            tilemap.gameObject.SetActive(false);
        }

        public Vector2Int GetGridPosition(Vector3 worldPosition) =>
            new Vector2Int((int) worldPosition.x, (int) worldPosition.y);

        #region Get Tile at Position

        public Tile GetTileAtPosition(Vector3 worldPosition) =>
            GetTileAtPosition(GetGridPosition(worldPosition));

        public Tile GetTileAtPosition(Vector2Int gridPos) => tiles[gridPos.x, gridPos.y];
        public Tile GetTileAtPosition(int x, int y) => tiles[x, y];

        #endregion

        public bool IsInGrid(Vector2Int gridPos) =>
            0 <= gridPos.x && gridPos.x < gridSize.x && 0 <= gridPos.y && gridPos.y < gridSize.y;

        public bool HasTile(Vector2Int gridPos)
        {
            if (!IsInGrid(gridPos))
                return false;
            return tiles[gridPos.x, gridPos.y] != null;
        }

        public void SetNeighborTilesWalkable(Tile tile, int range)
        {
            int x = tile.GridPos.x - range;
            int y = tile.GridPos.y - range;

            int length = range * 2 + 1;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Tile neighbor = GetTileAtPosition(x + i, y + j);
                    if (neighbor == null || neighbor == tile)
                        continue;
                    
                    if(GetManhattanDistance(tile, neighbor) <= range)
                        neighbor.SetWalkable();
                }
            }
        }

        #region Pathfinding

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

        public List<Direction> TempGetPath(Vector2Int startGridPos, Vector2Int targetGridPos)
        {
            Tile startTile = GetTileAtPosition(startGridPos);
            Tile endTile = GetTileAtPosition(targetGridPos);

            List<Tile> openList = new List<Tile>() {startTile};
            List<Tile> closedList = new List<Tile>();

            // Initialize every tiles
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Tile tile = GetTileAtPosition(x, y);
                    if (tile == null)
                        continue;

                    tile.GCost = int.MaxValue;
                    tile.previousTile = null;
                }
            }

            startTile.GCost = 0;
            startTile.HCost = GetManhattanDistance(startGridPos, targetGridPos);

            while (openList.Count > 0)
            {
                // Get Tile with lowest F cost
                Tile currentTile =
                    GetLowestFCostTile(
                        openList); //openList.Where(x => x.FCost == openList.Min(y => y.FCost)).ToList()[0];
                // Debug.Log(openList.ToSeparatedString(","));

                // If we reached target
                if (currentTile == endTile)
                {
                    return CalculatePath(endTile);
                }

                openList.Remove(currentTile);
                closedList.Add(currentTile);

                foreach (Tile neighborTile in GetTouchingTiles(currentTile))
                {
                    if (neighborTile == null)
                        continue;

                    // Don't consider tiles in closed list
                    if (closedList.Contains(neighborTile))
                        continue;

                    int tentativeGCost = currentTile.GCost + 1 /*GetManhattanDistance(currentTile, touchingTile)*/;
                    if (tentativeGCost < neighborTile.GCost)
                    {
                        // Update values of neighbor Tile
                        neighborTile.previousTile = currentTile;
                        neighborTile.GCost = tentativeGCost;
                        neighborTile.HCost = GetManhattanDistance(neighborTile, endTile);

                        if (!openList.Contains(neighborTile))
                            openList.Add(neighborTile);
                    }
                }
            }

            // No more tiles in the open list
            return null;
        }

        private Tile GetLowestFCostTile(List<Tile> tiles)
        {
            int lowestFCostIndex = 0;
            int lowestFCost = tiles[0].FCost;
            for (int i = 1; i < tiles.Count; i++)
            {
                if (tiles[i].FCost < lowestFCost)
                {
                    lowestFCost = tiles[i].FCost;
                    lowestFCostIndex = i;
                }
            }

            return tiles[lowestFCostIndex];
        }

        private List<Direction> CalculatePath(Tile endTile)
        {
            List<Tile> tilesPath = new List<Tile>() {endTile};

            Tile currentTile = endTile;
            while (currentTile.previousTile != null)
            {
                tilesPath.Add(currentTile.previousTile);
                currentTile = currentTile.previousTile;
            }

            tilesPath.Reverse();

            List<Direction> directions = new List<Direction>();
            for (int i = 0; i < tilesPath.Count - 1; i++)
            {
                Vector2Int gridDir = tilesPath[i + 1].GridPos - tilesPath[i].GridPos;
                directions.Add(Direction.FromVector2Int(gridDir));
            }

            return directions;
        }

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

        #endregion

        #region Get Manhattan Distance

        public int GetManhattanDistance(Vector2Int a, Vector2Int b) =>
            Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        public int GetManhattanDistance(Tile a, Tile b) =>
            GetManhattanDistance(a.GridPos, b.GridPos);
        public int GetManhattanDistance(Vector3 a, Vector3 b) => 
            GetManhattanDistance(GetGridPosition(a), GetGridPosition(b));
        #endregion

    }
}