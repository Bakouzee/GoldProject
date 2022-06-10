using System.Collections.Generic;
using GoldProject;
using GridSystem;
using UnityEngine;
using GoldProject.Rooms;

namespace Enemies.States
{
    public class EnemyInteractState : EnemyFollowedState
    {
        private IInteractable interactable;
        private Tile aimedTile;

        public EnemyInteractState(EnemyBase enemy, EnemyBaseState nextState, IInteractable interactable) : base(enemy,
            nextState)
        {
            if (interactable == null)
            {
                GoToNextState();
                return;
            }
            if (interactable is Curtain {IsOpened: true})
            {
                GoToNextState();
                return;
            }

            this.aimedTile = gridController.gridManager.FindClosestTile(interactable.Transform.position);
            if (!aimedTile)
            {
                GoToNextState();
                return;
            }

            DefinePath(aimedTile.GridPos);
            this.interactable = interactable;
        }

        public override void DoAction()
        {
            // Go to next state if
            // If curtain already opened
            if (interactable is Curtain {IsOpened: true})
            {
                GoToNextState();
                return;
            }

            // If direction queue is null
            if (directions == null)
            {
                interactable?.TryInteract();
                GoToNextState();
                return;
            }

            // If direction queue is empty
            if (directions.Count == 0)
            {
                interactable.TryInteract();
                GoToNextState();
                return;
            }


            gridController.Move(directions.Dequeue(), animator);
        }
    }
}