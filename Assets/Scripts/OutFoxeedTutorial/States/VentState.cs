using System;
using GridSystem;
using UnityEngine;

namespace OutFoxeedTutorial.States
{
    [System.Serializable]
    public class VentState : OutFoxeedTutorial.TutorialState
    {
        GameManager gameManager;
        [SerializeField]
        private GameObject[] vents;
        
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            gameManager = GameManager.Instance;
            foreach (var vent in vents)
            {
                Tile tile = GridManager.Instance.GetTileAtPosition(vent.transform.position + Vector3.left);
                if (!tile)
                    continue;
                tile.SetTutoHighlight(true);
            }

            NewVentManager.OnVentUsed += EndState;
        }

        protected override void EndState()
        {
            NewVentManager.OnVentUsed -= EndState;
            base.EndState();
        }
    }
}