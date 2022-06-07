using System.Collections;
using System.Collections.Generic;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Enemies.States
{
    public class BreakerExplorationState : EnemyFollowedState
    {
        private Curtain curtainTarget;
        private Vector2Int aimedPos;

        public BreakerExplorationState(EnemyBase enemy, EnemyBaseState nextState, Room room = null) : base(enemy,
            nextState)
        {
            SetTarget(room ?? enemy.CurrentRoom);
        }

        public override IEnumerator OnStateEnter()
        {
            SetPath();
            yield return null;
        }

        public override void DoAction()
        {
            if (directions.Count > 0)
                gridController.Move(directions.Dequeue());

            if (gridManager.GetManhattanDistance(transform.position, curtainTarget.transform.position) <= 1)
            {
                // Break curtain
                curtainTarget.Break();

                // Choose next room
                Room newRoom = RoomsManager.Instance.GetRandomRoom();
                while (newRoom != enemy.CurrentRoom && newRoom.curtains.Length == 0)
                    newRoom = RoomsManager.Instance.GetRandomRoom();

                SetTarget(RoomsManager.Instance.GetRandomRoom());
                SetPath();
            }
            else Debug.Log("Arrived but too far");
        }

        private void SetTarget(Room room)
        {
            if (room == null)
                return;

            this.curtainTarget = room.curtains[room.curtains.Length];
            this.aimedPos = gridManager.FindClosestTile(this.curtainTarget.transform.position).GridPos;
        }

        private void SetPath() => directions = new Queue<Direction>(gridManager.GetPath(gridPos, aimedPos));
    }
}