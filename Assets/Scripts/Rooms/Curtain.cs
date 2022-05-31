using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoldProject.Rooms
{
    public class Curtain : MonoBehaviour, IInteractable
    {
        static List<Curtain> curtains = new List<Curtain>();
        [SerializeField] private Animator animator;

        private bool opened;
        public bool IsOpened => opened;
        public System.Action onStateChanged;

        private void Awake() => curtains.Add(this);
        private void OnDestroy() => curtains.Remove(this);
        private void Start() => animator.SetBool("day", true);


        public bool IsInteractable => true;//opened;
        public void Interact()
        {
            SetOpened(!opened);
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
            
            animator.SetTrigger("open");
            //TODO: lights
            
            onStateChanged?.Invoke();
        }
        private void Close()
        {
            if (!opened)
                return;
            opened = false;
            
            animator.SetTrigger("close");
            //TODO: lights
            
            onStateChanged?.Invoke();
        }

        public static void SetDay(bool day)
        {
            foreach (var curtain in curtains)
                curtain.animator.SetBool("day", day);
        }
    }
}