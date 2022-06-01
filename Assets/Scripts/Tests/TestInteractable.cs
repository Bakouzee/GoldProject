using GridSystem;
using UnityEngine;

namespace GoldProject.Tests
{
    public class TestInteractable : MonoBehaviour, IInteractable
    {
        private GridController gridController;
        private void Start() => gridController = new GridController(transform);

        public bool IsInteractable => true;
        public void Interact()
        {
            Debug.Log($"Interacted with {name}");
        }
    }
}