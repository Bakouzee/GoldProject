using UnityEngine;

namespace GoldProject
{
    public interface IInteractable
    {
        public Transform Transform { get; }
        public bool IsInteractable { get; }
        public bool NeedToBeInRange { get; }
        public bool TryInteract();
    }
}