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
            lastChasePos = GridManager.Instance.GetGridPosition(chaseEntity.transform.position);
            directions = new Queue<Direction>(GridManager.Instance.GetPath(gridController.gridPosition,GridManager.Instance.GetGridPosition(chaseEntity.transform.position)));

            if(directions.Count > 0)
                gridController.Move(directions.Dequeue());

            
        }

        public override IEnumerator OnStateExit()  {
          //  directions = new Queue<Direction>(GridManager.Instance.GetPath(gridPos, lastChasePos));
            yield return null;
        }

    }
}
