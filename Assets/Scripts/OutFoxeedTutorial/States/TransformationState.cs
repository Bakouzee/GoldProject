using Enemies;
using GridSystem;
using UnityEngine;

namespace OutFoxeedTutorial.States
{
    [System.Serializable]
    public class TransformationState : OutFoxeedTutorial.TutorialState
    {
        [SerializeField] private Transform[] enemiesSpawnPoints;
        [SerializeField] private EnemyBase enemyPrefab;
        private EnemyBase[] enemies;

        private GridManager gridManager;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            var player = PlayerManager.Instance.Player;
            player.CanTransform = true;
            player.Transform();
            
            // Spawn enemies
            foreach (var enemiesSpawnPoint in enemiesSpawnPoints)
            {
                EnemyBase enemyInstance = Object.Instantiate(enemyPrefab, enemiesSpawnPoint.position, Quaternion.identity);
                // enemyInstance.enabled = false;
            }
            enemies = Object.FindObjectsOfType<EnemyBase>(true);
            
            // Set tuto highlights
            gridManager = GridManager.Instance;
            foreach (var enemy in enemies)
            {
                Tile tile = gridManager.GetTileAtPosition(enemy.transform.position);
                if (!tile)
                    continue;
                tile.SetTutoHighlight(true);
            }
        }

        public override void OnStateUpdate()
        {
            Tile.ResetTutoTiles();
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] == null)
                {
                    EndState();
                    return;
                }
                else
                {
                    Tile tile = gridManager.GetTileAtPosition(enemies[i].transform.position);
                    if (!tile)
                        continue;
                    tile.SetTutoHighlight(true);
                }
            }
        }
    }
}