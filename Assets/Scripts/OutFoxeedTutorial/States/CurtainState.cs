using GoldProject.Rooms;
using GridSystem;
using UnityEngine;

namespace OutFoxeedTutorial.States
{
    [System.Serializable]
    public class CurtainState : OutFoxeedTutorial.TutorialState
    {
        [SerializeField] private Curtain curtain;
        private Tile tileNextCurtain;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            curtain.SetOpened(true);
            tileNextCurtain = GridManager.Instance.FindClosestTile(curtain.transform.position);
            tileNextCurtain.SetTutoHighlight(true);
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            if (!curtain.IsOpened)
                EndState();
        }
    }
}