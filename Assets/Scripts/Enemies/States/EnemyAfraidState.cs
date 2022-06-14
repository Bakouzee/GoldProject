using System.Collections.Generic;
using System.Linq;
using GridSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace Enemies.States
{
    /// <summary>
    /// State where the enemy runs away from
    /// a frightening source for a certain period of time
    /// </summary>
    public class EnemyAfraidState : EnemyFollowedState
    {
        protected Transform frighteningSource;
        private Vector2 frighteningSourceLastPos;
        
        private int numberOfTurn;
        private int turnCounter;

        private System.Action endAction;
        
        public EnemyAfraidState(EnemyBase enemy, Transform frighteningSource, int numberOfTurn, EnemyBaseState nextState) : base(enemy, nextState)
        {
            this.frighteningSource = frighteningSource;
            this.frighteningSourceLastPos = this.frighteningSource.position;

            this.numberOfTurn = numberOfTurn;
            this.turnCounter = 0;
        }

        public EnemyAfraidState(EnemyBase enemy, Transform frighteningSource, int numberOfTurn, EnemyBaseState nextState,System.Action endAction) : this(enemy,frighteningSource,numberOfTurn,nextState)
        {
            this.endAction = endAction;
        }

        public override IEnumerator OnStateEnter()
        {
            enemy.Afraid = true;
            enemy.SetActiveLight(false);
            enemy.spriteRenderer.color = Color.blue;
            yield return null;
        }

        public override void DoAction()
        {
            if (frighteningSource) frighteningSourceLastPos = frighteningSource.position;
            Vector2 runningDir = (Vector2)transform.position - frighteningSourceLastPos;
            Direction runDirection = Direction.FromVector2(runningDir);

            var bestRunningDirections = GetBestRunningDirections();

            foreach (var runningDirection in bestRunningDirections)
            {
                if (gridController.Move(runningDirection, animator))
                    break;
            }

            turnCounter++;
            if (turnCounter >= numberOfTurn)
            {
                Debug.Log("go to next state");
                GoToNextState();
            }
        }

        public override IEnumerator OnStateExit()
        {
            enemy.Afraid = false;
            enemy.SetActiveLight(true);
            enemy.spriteRenderer.color = Color.white;
            endAction?.Invoke();
            // Debug.Log("do end action");
            yield return null;
        }

        private Vector2Int[] GetBestRunningDirections()
        {
            Vector2Int[] allDirections = new Vector2Int[] {Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right};

            // Get current frigthening source grid pos
            Vector2 frighteningSourcePosition = frighteningSource != null ? frighteningSource.position : frighteningSourceLastPos;
            Tile tile = gridManager.GetTileAtPosition(frighteningSourcePosition);
            if (!tile) tile = gridManager.FindClosestTile(frighteningSourcePosition);
            Vector2Int currentFrighteningSourceGridPos = tile.GridPos;
            
            // Initialize array containing directions and values
            (Vector2Int, int)[] tuples = new (Vector2Int, int)[allDirections.Length];
            for (int i = 0; i < allDirections.Length; i++)
            {
                tuples[i].Item1 = allDirections[i];

                Vector2Int temp = gridController.gridPosition + tuples[i].Item1;
                if (gridManager.HasTile(temp))
                {
                    // Calculate the number of move needed to join the frighteningSource from current position + direction
                    tuples[i].Item2 = gridManager.GetPath(enemy.gridController.gridPosition + tuples[i].Item1,
                        currentFrighteningSourceGridPos).Count;
                }
                else
                    tuples[i].Item2 = 0;
            }

            // Order them
            var orderedTuples = tuples.OrderBy(tuple => tuple.Item2).Reverse().ToList();

            // Return them all except the last one (=the worst one)
            Vector2Int[] results = new Vector2Int[orderedTuples.Count() - 1];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = orderedTuples[i].Item1;
            }

            return results;
        }
    }
}