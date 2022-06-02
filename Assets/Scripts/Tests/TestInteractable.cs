using GridSystem;
using UnityEngine;

namespace GoldProject.Tests
{
    public class TestInteractable : MonoBehaviour, IInteractable
    {
        private GridController gridController;
        private void Start() => gridController = new GridController(transform);

        public Transform Transform => transform;
        public bool IsInteractable => true;
        public bool NeedToBeInRange => true;

        public void Interact()
        {
            Debug.Log($"Interacted with {name}");
        }
    }
}