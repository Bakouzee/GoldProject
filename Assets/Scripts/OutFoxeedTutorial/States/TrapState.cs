using Enemies;
using GoldProject.FrighteningEvent;
using GridSystem;
using UnityEngine;

namespace OutFoxeedTutorial.States
{
    [System.Serializable]
    public class TrapState : OutFoxeedTutorial.TutorialState
    {
        private FrighteningEventBase[] frighteningEvents;
        [SerializeField] private Behaviour[] toActivate;
        [SerializeField] private EnemyBase enemyToFrighten;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            frighteningEvents = Object.FindObjectsOfType<FrighteningEventBase>();
            var gridManager = GridManager.Instance;
            foreach (var frighteningEvent in frighteningEvents)
            {
                Tile closestTile = gridManager.FindClosestTile(frighteningEvent.transform.position);
                if (!closestTile)
                    continue;
                closestTile.SetTutoHighlight(true);
            }

            enemyToFrighten.enabled = false;
            
            for (int i = 0; i < toActivate.Length; i++)
            {
                if (toActivate[i] == null)
                    continue;
                toActivate[i].enabled = true;
            }
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();
            foreach (var frighteningEvent in frighteningEvents)
            {
                if (frighteningEvent.IsTriggered)
                {
                    enemyToFrighten.enabled = true;
                    enemyToFrighten.GetAfraid(PlayerManager.Instance.transform);
                    EndState();
                }
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            foreach (var behaviour in toActivate)
                behaviour.enabled = false;
        }
    }
}