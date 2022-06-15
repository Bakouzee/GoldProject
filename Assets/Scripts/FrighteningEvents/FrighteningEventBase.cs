using System;
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
        protected GridController gridController;
        protected Rooms.Room currentRoom;

        public Rooms.Room CurrentRoom { get { return currentRoom; } set { currentRoom = value; } }

        public int distanceToBeScared = 6;

        private bool isTriggered = false;
        public bool IsTriggered => isTriggered;
        private bool inProgress = false;
        public bool IsInProgress => inProgress;

        public static System.Action<FrighteningEventBase> OnFrighteningEventTriggered;
        public static System.Action<FrighteningEventBase> OnFrighteningEventRearmed;

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
            isTriggered = true;
            inProgress = true;
            OnFrighteningEventTriggered?.Invoke(this);
            yield return DoActionCoroutine();
            inProgress = false;
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
            isTriggered = false;
            inProgress = true;
            OnFrighteningEventRearmed?.Invoke(this);
            yield return UndoActionCoroutine();
            inProgress = false;
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distanceToBeScared);
        }

        // Temporary for debug reasons
        public Transform Transform => transform;
        public bool IsInteractable
        {
            get
            {
                if (inProgress)
                    return false;
                
                switch (GameManager.dayState)
                {
                    case GameManager.DayState.DAY:
                        return !IsTriggered;
                    case GameManager.DayState.NIGHT:
                        if (isTriggered)
                            return GridManager.Instance.GetManhattanDistance(transform.position,
                                PlayerManager.Instance.transform.position) <= 1;
                        return true;
                    default:
                        return false;
                }
                
            }
        }

        protected bool needToBeInRange = true;
        public bool NeedToBeInRange => needToBeInRange;

        public virtual bool TryInteract()
        {
            if (!IsInteractable)
                return false;
            return true;
            
            // To override
        }
    }
}