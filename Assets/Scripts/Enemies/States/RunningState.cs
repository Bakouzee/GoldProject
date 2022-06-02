using System.Collections.Generic;
using System.Linq;
using GridSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies.States
{
    public class RunningState : EnemyFollowedState
    {
        protected Transform frighteningSource;
        private Vector2 frighteningSourceLastPos;
        
        private int numberOfTurn;
        private int turnCounter;
        
        public RunningState(EnemyBase enemy, Transform frighteningSource, int numberOfTurn, EnemyBaseState nextState) : base(enemy, nextState)
        {
            this.frighteningSource = frighteningSource;
            this.frighteningSourceLastPos = this.frighteningSource.position;
        
            this.numberOfTurn = numberOfTurn;
            this.turnCounter = 0;
        }

        public override void DoAction()
        {
            Vector2 runningDir = transform.position - frighteningSource.position;
            Direction runDirection = Direction.FromVector2(runningDir);

            var bestRunningDirections = GetBestRunningDirections();

            foreach (var runningDirection in bestRunningDirections)
            {
                if (gridController.Move(runningDirection))
                    break;
            }

            turnCounter++;
            if (turnCounter >= numberOfTurn)
            {
                GoToNextState();
            }
        }

        private Vector2Int[] GetBestRunningDirections()
        {
            Vector2Int[] allDirections = new Vector2Int[] {Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right};

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
                        gridManager.GetGridPosition(frighteningSource.position)).Count;
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