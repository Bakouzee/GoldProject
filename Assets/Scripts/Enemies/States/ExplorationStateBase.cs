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
            var randomRoom = RoomsManager.Instance.GetRandomRoom();

            // Initiliaze path points
            pathPointsVerified = new bool[randomRoom.pathPoints.Length];

            // Choose target with random index
            ChooseTargetOfIndex(randomRoom, UnityEngine.Random.Range(0, randomRoom.pathPoints.Length));

        }

        public override IEnumerator OnStateEnter()
        {
            yield return null;
        }

        public override void DoAction()
        {
            // Try place a garlic, if we can -> don't move
            if (TryPlaceGarlic())
                return;

            // Move if has direction
            if (directions.Count > 0)
                gridController.Move(directions.Dequeue(), animator);

            // If no more directions, choose next target
            if (directions.Count == 0)
                ChooseNextTarget();
        }

        #region Exploration helpers

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
            // Debug.Log($"{enemy.name}: {room.pathPoints.Length} // {index}");
            targetGridPos = GridManager.Instance.GetGridPosition(room.pathPoints[index].position);

            // Define path
            if (!DefinePath(targetGridPos))
                // If it didn't work --> change of room
                ChangeOfRoom();
        }

        #endregion

        #region Place Garlic functions

        bool TryPlaceGarlic()
        {
            if (enemy.HasPlacedGarlic)
                return false;

            var position = transform.position;

            // Try with closest vent
            var ventManager = enemy.CurrentRoom.GetClosestVent(position);
            if (ventManager && GarlicRandom())
            {
                // If vent close enough
                if (gridManager.GetManhattanDistance(position, ventManager.transform.position) < enemy.detectionRangeForGarlic)
                {
                    PlaceGarlic();
                    return true;
                }
            }

            // Else, try with closest open curtain
            var closestCurtain = enemy.CurrentRoom.GetClosestCurtain(position);
            if (closestCurtain && GarlicRandom())
            {
                // If curtain close enough and opened
                if (gridManager.GetManhattanDistance(transform.position, closestCurtain.transform.position) <
                    enemy.detectionRangeForGarlic && closestCurtain.IsOpened)
                {
                    PlaceGarlic();
                    return true;
                }
            }

            return false;
        }

        bool GarlicRandom() => UnityEngine.Random.value <= enemy.garlicProba;

        void PlaceGarlic()
        {
            Debug.Log("Placed garlic");

            // Spawn and add garlic to the current room
            enemy.CurrentRoom.garlics.Add(Object.Instantiate(enemy.garlicPrefab, transform.position,
                Quaternion.identity));

            enemy.HasPlacedGarlic = true;
        }

        #endregion
    }
}