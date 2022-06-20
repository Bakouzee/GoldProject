using System.Collections.Generic;
using GridSystem;
using TMPro;
using UnityEngine;

namespace OutFoxeedTutorial
{
    [System.Serializable]
    public abstract class TutorialState
    {
        protected OutFoxeedTutorial.TutorialManager manager 
            => OutFoxeedTutorial.TutorialManager.Instance;
        
        public string[] texts;
        private int currentTextIndex;
        public string CurrentText => texts[currentTextIndex % texts.Length];
        public void NextText()
        {
            if (currentTextIndex + 1 == texts.Length)
                return;
            currentTextIndex++;
        }

        public virtual void OnStateEnter() {}
        public virtual void OnStateUpdate() {}
        public virtual void OnStateExit() => Tile.ResetTutoTiles();

        protected virtual void EndState()
        {
            manager.GoToNextState();
        }
    }
}