using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GridSystem
{
    public class Tile : MonoBehaviour
    {
        private static List<Tile> walkableTiles = new List<Tile>();
        private static List<Tile> tutoTiles = new List<Tile>();

        // Highlights
        [FormerlySerializedAs("highlight")] [SerializeField]
        private GameObject mouseHighlight;
        [SerializeField] private GameObject walkableHighlight;
        private bool walkable;
        [SerializeField] private GameObject tutoHighlight;
        
        private Vector2Int gridPos;
        public Vector2Int GridPos => gridPos;

        public void SetGridPos(Vector2Int vec)
        {
            gridPos = vec;
            gameObject.name = $"{{{vec.x} ; {vec.y}}}";
        }

        // Astar Variables

        #region Astar variables

        [HideInInspector] public int GCost { get; set; } // Distance between begin tile to actual tile
        [HideInInspector] public int HCost; // Distance between actual tile and end tile (without obstacle)
        public int FCost => GCost + HCost;
        [HideInInspector] public Tile previousTile;

        #endregion

        private void Start()
        {
            if (mouseHighlight.activeSelf) mouseHighlight.SetActive(false);
        }

        #region Walkable highlighting
        public bool IsWalkable => walkable;
        public void SetWalkable()
        {
            if (walkable)
                return;
            
            walkable = true;
            walkableHighlight.SetActive(true);
            walkableTiles.Add(this);
        }
        public static void ResetWalkableTiles()
        {
            foreach (var tile in walkableTiles)
            {
                if (tile == null)
                    continue;
                
                tile.walkable = false;
                if(tile.walkableHighlight.activeSelf) tile.walkableHighlight.SetActive(false);
                if(tile.mouseHighlight.activeSelf) tile.mouseHighlight.SetActive(false);
            }
            walkableTiles = new List<Tile>();
        }
        #endregion

        #region Mouse highlighting
        private void OnMouseEnter()
        {
            // Do nothing if clicking on UI
            if (GameManager.eventSystem.IsPointerOverGameObject())
                return;
            
            mouseHighlight.SetActive(true);
        }

        private void OnMouseExit()
        {
            // Do nothing if clicking on UI
            if (GameManager.eventSystem.IsPointerOverGameObject())
                return;

            mouseHighlight.SetActive(false);
        }
        #endregion

        #region Tuto highlighting
        public static void ResetTutoTiles()
        {
            foreach (var tile in tutoTiles)
            {
                if (!tile)
                    continue;
                if(tile.tutoHighlight.activeSelf)
                    tile.tutoHighlight.SetActive(false);
            }
            tutoTiles.Clear();
        }

        public void SetTutoHighlight(bool enable)
        {
            if(enable) tutoTiles.Add(this);
            else if (tutoTiles.Contains(this)) tutoTiles.Remove(this);
            
            if(tutoHighlight.activeSelf != enable)
                tutoHighlight.SetActive(enable);
        }
        #endregion
    }
}
