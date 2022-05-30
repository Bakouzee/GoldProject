using System.Collections;
using System.Collections.Generic;
using GoldProject.Rooms;
using GridSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies.States
{
    public class ExplorationStateBase : EnemyBaseState
    {
        protected Vector2Int targetGridPos;
        private bool[] pathPointsVerified;

        public ExplorationStateBase(EnemyBase enemy) : base(enemy)
        {
            // Initiliaze path points
            pathPointsVerified = new bool[enemy.CurrentRoom.pathPoints.Length];
            
            // Choose target with random index
            ChooseTargetOfIndex(RoomsManager.Instance.GetRandomRoom(), 
            UnityEngine.Random.Range(0, enemy.CurrentRoom.pathPoints.Length));
        }

        public override IEnumerator OnStateEnter()
        {
            yield return null;
        }

        public override void DoAction()
        {
            // Debug.Log("Do action");
            enemy.Move(directions.Dequeue());
            if(directions.Count == 0)
                ChooseNextTarget();
        }

        private void ChooseNextTarget()
        {
            // Debug.Log("Choose next");
            
            // Get indexes of path points not already used
            List<int> availableIndexes = new List<int>();
            for (int i = 0; i < pathPointsVerified.Length; i++)
            {
                if (!pathPointsVerified[i])
                {
                    availableIndexes.Add(i);
                }
            }

            // If no path point available --> change of room
            if (availableIndexes.Count == 0)
            {
                ChangeOfRoom();
                return;
            }

            // Choose a random index
            int chosenIndex = availableIndexes[UnityEngine.Random.Range(0, availableIndexes.Count)];
            
            // Set target and path
            ChooseTargetOfIndex(enemy.CurrentRoom, chosenIndex);
        }


        private void ChangeOfRoom()
        {
            // Debug.Log("Change of room");
            
            // Get random room (exluding the current)
            RoomsManager roomsManager = RoomsManager.Instance;
            Room newRoom = null;
            while (newRoom == null || newRoom == enemy.CurrentRoom)
                newRoom = roomsManager.GetRandomRoom();

            // Reset verified path points array
            pathPointsVerified = new bool[newRoom.pathPoints.Length];
            int index = UnityEngine.Random.Range(0, newRoom.pathPoints.Length);
            
            // Set target and path
            ChooseTargetOfIndex(newRoom, index);            
        }
        
        /// <summary>
        /// Helper for target choice
        /// Set path points uses, set targetGridPos and path
        /// </summary>
        /// <param name="room"></param>
        /// <param name="index"></param>
        private void ChooseTargetOfIndex(Room room, int index)
        {
            // Set this index as used
            pathPointsVerified[index] = true;

            // Set target and path
            targetGridPos = GridManager.Instance.GetGridPosition(room.pathPoints[index].position);
            DefinePath(targetGridPos);
        }
    }
}
