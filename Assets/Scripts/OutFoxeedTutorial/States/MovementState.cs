using GridSystem;
using UnityEngine;

namespace OutFoxeedTutorial.States
{
    [System.Serializable]
    public class MovementState : OutFoxeedTutorial.TutorialState
    {
        [SerializeField] private Vector2Int finalGridPos;
        private GridController playerGridController;
        
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            playerGridController = PlayerManager.Instance.Player.gridController;
            GridManager.Instance.GetTileAtPosition(finalGridPos).SetTutoHighlight(true);
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            if(playerGridController.gridPosition == finalGridPos)
                EndState();
        }
    }
}