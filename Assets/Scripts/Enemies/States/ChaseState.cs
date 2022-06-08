using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject;
using GridSystem;

namespace Enemies.States {
    public class ChaseState : EnemyBaseState {

        private Entity chaseEntity;
        private Vector2Int lastChasePos;

        public EnemyBase chief;

        public ChaseState(EnemyBase enemy,Entity chaseEntity, EnemyBase chief) : base(enemy) {
            this.chaseEntity = chaseEntity;
            this.chief = chief;
        }

        public override IEnumerator OnStateEnter() { 
            yield return null;
        }

        public override void DoAction() {
            
            // Attack entity if in range
            if (gridManager.GetManhattanDistance(chaseEntity.gridController.gridPosition, gridPos) <= 1)
            {
                // this is hardcoded, not very good. It doesn't depends on the current chased entity -> bad
                PlayerManager.Instance.PlayerHealth.Death();
                return;
            }
            
            // Else, move towards entity
            lastChasePos = GridManager.Instance.GetGridPosition(chaseEntity.transform.position);
            directions = new Queue<Direction>(GridManager.Instance.GetPath(gridController.gridPosition,GridManager.Instance.GetGridPosition(chaseEntity.transform.position)));
            if(directions.Count > 0)
                gridController.Move(directions.Dequeue(), animator);
        }

        public override IEnumerator OnStateExit()  {
            Debug.Log("exit: " + enemy + " chief " + chief);
            if (enemy == chief)
            {
                Debug.Log("exit enter");
                directions = new Queue<Direction>(GridManager.Instance.GetPath(gridPos, lastChasePos));
                Debug.Log("size " + directions.Count);
            }
            yield return null;
        }

    }
}
