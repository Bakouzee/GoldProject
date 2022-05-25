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
        }

        private void OnMouseDown()
        {
            GridManager.Instance.GetPath(GameObject.FindGameObjectWithTag("Player").transform.position,transform.position);
        }

        private void OnMouseExit()
        {
            highlight.SetActive(false);
        }
    }
}