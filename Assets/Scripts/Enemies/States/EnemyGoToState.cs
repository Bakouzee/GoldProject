using System.Collections;
using System.Collections.Generic;
using GridSystem;
using UnityEngine;

namespace Enemies.States
{
    public class EnemyGoToState : EnemyFollowedState
    {
        private Vector2Int aimedGridPos;
        private System.Action onArrived;
        
        public EnemyGoToState(EnemyBase enemy, Vector2Int aimedGridPos, System.Action onArrived, EnemyBaseState nextState) : base(enemy, nextState)
        {
            this.aimedGridPos = aimedGridPos;
            this.onArrived = onArrived;
        }

        public override IEnumerator OnStateEnter()
        {
            directions = new Queue<Direction>(gridManager.GetPath(gridPos, aimedGridPos));
            yield return null;
        }
        
        public override void DoAction()
        {
            // Move
            if (directions.Count > 0)
                gridController.Move(directions.Dequeue(), animator);
            
            // If arrived, go to next state and call onArrived
            if (directions.Count == 0)
            {
                onArrived?.Invoke();
                GoToNextState();
            }
        }
    }
}