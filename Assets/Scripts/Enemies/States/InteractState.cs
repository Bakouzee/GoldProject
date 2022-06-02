using System.Collections.Generic;
using GoldProject;
using GridSystem;
using UnityEngine;

namespace Enemies.States
{
    public class InteractState : EnemyFollowedState
    {
        private IInteractable interactable;
        private Tile aimedTile;

        public InteractState(EnemyBase enemy, EnemyBaseState nextState, IInteractable interactable) : base(enemy,
            nextState)
        {
            this.aimedTile = gridController.gridManager.FindClosestTile(interactable.Transform.position);
            if (aimedTile == null)
            {
                GoToNextState();
                return;
            }

            directions = new Queue<Direction>(gridManager.GetPath(gridPos, aimedTile.GridPos));
        }

        public override void DoAction()
        {
            // If no more
            if (directions.Count == 0)
            {
                interactable.Interact();
                GoToNextState();
                return;
            }

            gridController.Move(directions.Dequeue());
        }
    }
}