using UnityEngine;

namespace GoldProject.Rooms
{
    public class Curtain : MonoBehaviour, IInteractable
    {
        private bool opened;
        public bool IsOpened => opened;
        public System.Action onStateChanged;

        public bool IsInteractable => opened;
        public void Interact()
        {
            Close();
        }

        public void SetOpened(bool newOpened)
        {
            if(newOpened) Open();
            else Close();
        }
        private void Open()
        {
            if (opened)
                return;
            opened = true;
            
            //TODO: animations + lights
            
            onStateChanged?.Invoke();
        }
        private void Close()
        {
            if (!opened)
                return;
            opened = false;
            
            //TODO: animations + lights
            
            onStateChanged?.Invoke();
        }
    }
}