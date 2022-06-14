using System;
using System.Collections.Generic;

namespace Enemies
{
    public static class EnemyManager
    {
        public static List<EnemyBase> enemies = new List<EnemyBase>();
        public static int enemiesCount => enemies.Count; 
        public static List<KnightEvent> knights = new List<KnightEvent>();
        public static int enemyKilled;
        public static int enemyAfraid;


        /// <summary>Event that is called by an enemy when he is killed by the player</summary>
        public static Action<EnemyBase> OnEnemyKilled;

        /// <summary>Event that is called by an enemy when he dies</summary>
        public static Action<EnemyBase> OnEnemyDeath;

        /// <summary>Event called by an enemy when he leaves the game</summary>
        public static Action<EnemyBase> OnEnemyStartLeaving;

        /// <summary>Event called by OnEnemyLeft and OnEnemyDeath</summary>
        public static Action<EnemyBase> OnEnemyDisappeared;

        /// <summary>
        /// Method needed to be called at the beginning of the game to reset stats
        /// </summary>
        public static void Reset()
        {
            enemies.Clear();
            knights.Clear();
            
            enemyKilled = 0;
            enemyAfraid = 0;

            OnEnemyKilled = enemy =>
            {
                OnEnemyDisappeared?.Invoke(enemy);
                enemyKilled++;
            };
            
            OnEnemyDeath = enemy => OnEnemyDisappeared?.Invoke(enemy);
            
            OnEnemyStartLeaving = enemy =>
            {
                OnEnemyDisappeared?.Invoke(enemy);
                enemyAfraid++;
            };

            OnEnemyDisappeared = null;
        }
    }
}