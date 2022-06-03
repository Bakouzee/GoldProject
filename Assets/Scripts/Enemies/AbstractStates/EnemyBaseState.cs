using System.Collections;
using System.Collections.Generic;
using GridSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Base class of all enemy states
    /// </summary>
    public abstract class EnemyBaseState
    {
        protected EnemyBase enemy;
        protected GameObject gameObject => enemy.gameObject;
        protected Transform transform => enemy.transform;
        protected GridController gridController => enemy.GridController;
        protected GridManager gridManager => gridController.gridManager;
        protected Vector2Int gridPos => gridController.gridPosition;

        // For movements
        public Queue<Direction> directions;


        protected void DefinePath(Vector2Int targetGridPos)
        {
            // Debug.Log($"{enemy.GridPosition} // {targetGridPos}");
            directions = new Queue<Direction>(GridManager.Instance
                .GetPath(gridPos, targetGridPos));
        }

        public EnemyBaseState(EnemyBase enemy)
        {
            this.enemy = enemy;
        }

        public virtual IEnumerator OnStateEnter()
        {
            yield return null;
        }
        
        public abstract void DoAction();
        
        public virtual void OnStateUpdate()
        {
            
        }

        public virtual IEnumerator OnStateExit()
        {
            yield return null;
        }
    }
}