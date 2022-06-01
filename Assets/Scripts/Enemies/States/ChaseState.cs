using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject;
using GridSystem;

namespace Enemies.States {
    public class ChaseState : EnemyBaseState {

        private Entity chaseEntity;

        public ChaseState(EnemyBase enemy,Entity chaseEntity) : base(enemy) {
            this.chaseEntity = chaseEntity;
        }

        public override IEnumerator OnStateEnter() { 
            yield return null;
        }

        public override void DoAction() {

            directions = new Queue<Direction>(GridManager.Instance.GetPath(gridController.gridPosition,GridManager.Instance.GetGridPosition(chaseEntity.transform.position)));
            gridController.Move(directions.Dequeue());

        }

    }
}
