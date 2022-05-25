using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GridSystem
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject highlight;

        private void Start()
        {
            if(highlight.activeSelf) highlight.SetActive(false);
        }

        private void OnMouseEnter()
        {
            highlight.SetActive(true);
        }

        private void OnMouseExit()
        {
            highlight.SetActive(false);
        }
    }
}