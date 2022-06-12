using System.Collections;
using UnityEngine;
using GoldProject;
using GridSystem;

namespace Enemies.States {
    public class EnemyChaseState : EnemyFollowedState {

        private Entity chaseEntity;

        private Vector2Int _lastTargetGridPos;
        private Vector2Int LastTargetGridPos
        {
            set
            {
                _lastTargetGridPos = value;
                DefinePath(_lastTargetGridPos);
            }
        }
        
        private bool _hasSight;
        private bool HasSight
        {
            get => _hasSight;
            set
            {
                _hasSight = value;
                if (!_hasSight) turnSinceSightLost = 1;
            }
        }
        private int turnSinceSightLost;
        private readonly int TURNS_TO_STOP_POS_UPDATE = 3;

        public EnemyChaseState(EnemyBase enemy,Entity chaseEntity, EnemyBaseState onChaseFinished) : base(enemy, onChaseFinished) {
            this.chaseEntity = chaseEntity;
            this.LastTargetGridPos = chaseEntity.gridController.gridPosition;
        }

        public override IEnumerator OnStateEnter()
        {
            enemy.Chasing = true;
            this.HasSight = true;
            AlertOtherEnemies();
            yield return null;
        }

        public override void DoAction() 
        {
            // If has sight on the entity chased
            if (HasSight)
            {
                // Attack entity if in range
                if (gridManager.GetManhattanDistance(chaseEntity.gridController.gridPosition, gridPos) <= 1)
                {
                    // this is hardcoded, not very good. It doesn't depends on the current chased entity -> bad
                    PlayerManager.Instance.PlayerHealth.Death();
                    return;
                }
                
                // If entity in the same room, alert other enemies + update last position
                if (chaseEntity.CurrentRoom == enemy.CurrentRoom)
                {
                    AlertOtherEnemies();

                    LastTargetGridPos = chaseEntity.gridController.gridPosition;
                }
                // Else, marked as sight lost
                else
                {
                    HasSight = false;
                }
            }
            
            // If hasn't the sight on the entity chased
            if(!HasSight)
            {
                // Check if entity is in sight and update if needed
                if (enemy.HasLineOfSight(chaseEntity.transform.position))
                    HasSight = true;
                // Increment the number of turn without having the entity in sight
                else
                {
                    turnSinceSightLost++;
                    // Update chased entity last pos only
                    // if we lost sight not too long ago
                    if (turnSinceSightLost <= TURNS_TO_STOP_POS_UPDATE)
                    {
                        LastTargetGridPos = chaseEntity.gridController.gridPosition;
                    }
                }
            }

            // Move
            if (directions.Count > 0)
                gridController.Move(directions.Dequeue(), animator);
            // If already arrived at last entity grid pos
            // (we know that the enemy is not next)
            else
            {
                // If enemy is in sight now that we arrived at last know entity grid pos
                if (enemy.HasLineOfSight(chaseEntity.transform.position))
                {
                    // Update everything and continue chasing
                    HasSight = true;
                    LastTargetGridPos = chaseEntity.gridController.gridPosition;
                    if (directions.Count > 0)
                        gridController.Move(directions.Dequeue(), animator);
                }
                else
                {
                    enemy.SetState(new EnemyLookState(
                        enemy: enemy,
                        maxTurnDuration: 3,
                        playerFoundState: this,
                        playerNotFoundState:nextState
                        )
                    );
                }
            }
        }

        public override IEnumerator OnStateExit()
        {
            enemy.Chasing = false;
            yield return null;
        }

        private void AlertOtherEnemies()
        {
            foreach (var otherEnemy in enemy.CurrentRoom.enemies)
            {
                if (otherEnemy.Chasing)
                    continue;
                otherEnemy.SetState(new EnemyChaseState(otherEnemy, chaseEntity, otherEnemy.CurrentState));
            }
        }

    }
}
