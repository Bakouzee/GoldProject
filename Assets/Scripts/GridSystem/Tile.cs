using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GridSystem
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject highlight;

        // Astar Variables
        [HideInInspector]
        public int GCost { get; set; } // Distance between begin tile to actual tile
        [HideInInspector]
        public int HCost; // Distance between actual tile and end tile (without obstacle)
        [HideInInspector]
        public int FCost { get => GCost + HCost; }

        private void Start()
        {
            if(highlight.activeSelf) highlight.SetActive(false);
        }

        private void OnMouseEnter()
        {
            highlight.SetActive(true);

            Vector2Int tileCoords = GridManager.Instance.GetTileCoords(this);
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "{" + tileCoords.x + ";" + tileCoords.y + "}";
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "G:" +GCost;
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "H:" + HCost;
            GameObject.FindObjectOfType<Canvas>().transform.GetChild(3).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "F:" + FCost;
        }

        private void OnMouseDown()
        {
            GridManager.Instance.GetPath(GameObject.FindGameObjectWithTag("Player").transform.position,transform.position);
            transform.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
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