using System;
using System.Collections;
using UnityEngine;

namespace Enemies.States
{
    public class EnemyAttractedState : EnemyGoToState
    {
        public EnemyAttractedState(EnemyBase enemy, Vector2Int aimedGridPos, Action onArrived, EnemyBaseState nextState) : base(enemy, aimedGridPos, onArrived, nextState)
        {
        }

        public override IEnumerator OnStateEnter()
        {
            enemy.Attracted = true;
            yield return base.OnStateEnter();
        }

        public override IEnumerator OnStateExit()
        {
            enemy.Attracted = false;
            yield return base.OnStateExit();
        }
    }
}