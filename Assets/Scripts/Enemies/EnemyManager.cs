using System;
using System.Collections.Generic;

namespace Enemies
{
    public static class EnemyManager
    {
        public static List<EnemyBase> enemies = new List<EnemyBase>();
        public static List<KnightEvent> knights = new List<KnightEvent>();

        public static void Reset()
        {
            enemyKilled = 0;
            enemyAfraid = 0;
        }

        public static int enemyKilled;
        public static int enemyAfraid;

        /// <summary>Event that is called by an enemy when he is killed by the player</summary>
        public static Action<EnemyBase> OnEnemyKilled = enemy =>
        {
            OnEnemyDisappeared?.Invoke(enemy);
            enemyKilled++;
        };
        
        /// <summary>Event that is called by an enemy when he dies</summary>
        public static Action<EnemyBase> OnEnemyDeath = enemy => OnEnemyDisappeared?.Invoke(enemy);

        /// <summary>Event called by an enemy when he leaves the game</summary>
        public static Action<EnemyBase> OnEnemyStartLeaving = enemy =>
        {
            OnEnemyDisappeared?.Invoke(enemy);
            enemyAfraid++;
        };
        
        /// <summary>Event called by OnEnemyLeft and OnEnemyDeath</summary>
        public static Action<EnemyBase> OnEnemyDisappeared;
    }
}
