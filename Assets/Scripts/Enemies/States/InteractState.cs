using System.Collections.Generic;
using GoldProject;
using GridSystem;
using UnityEngine;
using GoldProject.Rooms;
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
            this.interactable = interactable;
        }

        public override void DoAction() {
            // If no more
            if (directions.Count == 0) {
                if (interactable is Curtain && ((Curtain)interactable).IsOpened) {
                    GoToNextState();
                    return;
                }

                interactable.TryInteract();
                GoToNextState();
                return;
            }

            gridController.Move(directions.Dequeue());
        }
    }
}