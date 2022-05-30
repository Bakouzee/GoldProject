using GridSystem;
using UnityEngine;

namespace GoldProject.Tests
{
    public class TestInteractable : GridController, IInteractable
    {
        public bool IsInteractable => true;
        public void Interact()
        {
            Debug.Log($"Interacted with {name}");
        }
    }
}