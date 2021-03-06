using System;
using System.Collections;
using UnityEngine;

namespace Enemies.States
{
    public class EnemyLeaveState : EnemyGoToState
    {
        public EnemyLeaveState(EnemyBase enemy, Vector2Int aimedGridPos, Action onArrived = null) : base(enemy,
            aimedGridPos, onArrived, null)
        {
            this.onArrived += () =>
            {
                enemy.Leaving = false;
                GameObject.Destroy(enemy.gameObject);
            };
        }

        public override IEnumerator OnStateEnter()
        {
            EnemyManager.OnEnemyStartLeaving?.Invoke(enemy);
            enemy.Leaving = true;
            enemy.SetActiveLight(false);
            enemy.spriteRenderer.color = Color.blue;
            yield return base.OnStateEnter();
        }
    }
}