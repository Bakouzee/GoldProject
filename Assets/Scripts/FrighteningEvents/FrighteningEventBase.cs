using System.Collections;
using UnityEngine;

namespace GoldProject.FrighteningEvent
{
    public abstract class FrighteningEventBase : MonoBehaviour, IInteractable
    {
        protected bool isTriggered = false;
        public bool IsTriggered => isTriggered;
        protected bool inProgress = false;

        #region Do
        protected abstract IEnumerator DoActionCoroutine();
        private IEnumerator DoCouroutine()
        {
            inProgress = true;
            yield return DoActionCoroutine();
            inProgress = false;
            isTriggered = true;
        }
        public void Do()
        {
            if (inProgress || isTriggered)
                return;
            StartCoroutine(DoCouroutine());
        }
        #endregion

        #region Undo
        protected abstract IEnumerator UndoActionCoroutine();
        private IEnumerator UndoCoroutine()
        {
            inProgress = true;
            yield return UndoActionCoroutine();
            inProgress = false;
            isTriggered = false;
        }
        public void Undo()
        {
            if (inProgress || !isTriggered)
                return;
            StartCoroutine(UndoCoroutine());
        }
        #endregion

        public bool IsInteractable => true; //isTriggered && !inProgress;
        public virtual void Interact()
        {
            if (!IsInteractable)
                return;
            
            // To override
        }
    }
}