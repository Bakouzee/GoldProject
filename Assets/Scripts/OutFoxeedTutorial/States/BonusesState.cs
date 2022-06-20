using UnityEngine;

namespace OutFoxeedTutorial.States
{
    [System.Serializable]
    public class BonusesState : OutFoxeedTutorial.TutorialState
    {
        [SerializeField] float duration = 5f;
        private float timer;

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            timer += Time.deltaTime;
            if(timer >= duration)
                EndState();
        }
    }
}