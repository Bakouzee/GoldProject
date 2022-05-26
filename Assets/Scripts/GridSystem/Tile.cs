using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GridSystem
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject highlight;
        private Vector2Int gridPos;
        public Vector2Int GridPos => gridPos;
        public void SetGridPos(Vector2Int vec)
        {
            gridPos = vec;
            gameObject.name = $"{{{vec.x} ; {vec.y}}}";
        }

        // Astar Variables
        #region Astar variables
        [HideInInspector]
        public int GCost { get; set; } // Distance between begin tile to actual tile
        [HideInInspector]
        public int HCost; // Distance between actual tile and end tile (without obstacle)
        [HideInInspector]
        public int FCost { get => GCost + HCost; }

        #endregion

        private void Start()
        {
            if(highlight.activeSelf) highlight.SetActive(false);
        }

        private void OnMouseEnter()
        {
            highlight.SetActive(true);
            
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "{" + gridPos.x + ";" + gridPos.y + "}";
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "G:" +GCost;
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "H:" + HCost;
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "F:" + FCost;
        }

        private void OnMouseExit()
        {
            highlight.SetActive(false);


            GameObject.FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }
}