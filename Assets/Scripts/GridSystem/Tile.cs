using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GridSystem
{
    public class Tile : MonoBehaviour
    {
        private static List<Tile> walkableTiles = new List<Tile>();

        // Highlights
        [FormerlySerializedAs("highlight")] [SerializeField]
        private GameObject mouseHighlight;
        [SerializeField] private GameObject walkableHighlight;
        private bool walkable;
        
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
                tile.walkableHighlight.SetActive(false);
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

            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "{" + gridPos.x + ";" + gridPos.y + "}";
            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "G:" +GCost;
            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "H:" + HCost;
            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "F:" + FCost;
        }

        private void OnMouseExit()
        {
            // Do nothing if clicking on UI
            if (GameManager.eventSystem.IsPointerOverGameObject())
                return;

            mouseHighlight.SetActive(false);


            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            // GameObject.FindObjectOfType<Canvas>().transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        #endregion
    }
}
