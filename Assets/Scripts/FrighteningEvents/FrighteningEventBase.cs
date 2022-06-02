using System.Collections;
using GoldProject.Rooms;
using UnityEngine;
using GridSystem;

namespace GoldProject.FrighteningEvent
{
    /// <summary>
    /// Base class of a frightening traps
    /// Is designed to be inherited to create
    /// traps with different behaviours
    ///
    /// When inheriting from this class, the new child class
    /// just has to override the DoActionCoroutine and UndoActionCoroutine
    /// meant to trigger and rearm the trap
    ///
    /// Using this class from the outside just needs the calls of Do
    /// and Undo methods
    /// </summary>
    public abstract class FrighteningEventBase : MonoBehaviour, IInteractable
    {
        protected Rooms.Room currentRoom;
        protected GridController gridController;

        public Rooms.Room CurrentRoom { get { return currentRoom; } set { currentRoom = value; } }

        public int distanceToBeScared = 6;

        private bool isTriggered = false;
        public bool IsTriggered => isTriggered;
        private bool inProgress = false;
        public bool IsInProgress => inProgress;

        private void Start()
        {
            gridController = new GridController(transform);
        }

        #region Do
        // Has to be overriden, rule the activation of the trap
        protected abstract IEnumerator DoActionCoroutine();
        
        // Method meant to be called from the oustide to trigger a trap
        public void Do()
        {
            if (inProgress || isTriggered)
                return;
            StartCoroutine(DoCouroutine());
        }
        
        // Helpers of Do()
        private IEnumerator DoCouroutine()
        {
            inProgress = true;
            yield return DoActionCoroutine();
            inProgress = false;
            isTriggered = true;
        }
        #endregion

        #region Undo
        // Has to be overriden, rule the rearm of the trap
        protected abstract IEnumerator UndoActionCoroutine();
        
        // Method meant to be called from the oustide to rearm a trap
        public void Undo()
        {
            if ((inProgress || !isTriggered) && GameManager.dayState != GameManager.DayState.NIGHT)
                return;
            StartCoroutine(UndoCoroutine());
        }
        
        // Helper of Undo()
        private IEnumerator UndoCoroutine()
        {
            inProgress = true;
            yield return UndoActionCoroutine();
            inProgress = false;
            isTriggered = false;
        }
        #endregion

        // Temporary for debug reasons
        public Transform Transform => transform;
        public bool IsInteractable => true;
        public bool NeedToBeInRange => isTriggered;

        public virtual void Interact()
        {
            if (!IsInteractable)
                return;
            
            // To override
        }
    }
}