using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace GoldProject
{
    [CreateAssetMenu(fileName = "NewEnemyProfile", menuName = "EnemyProfile", order = 0)]
    public class EnemyProfile : ScriptableObject
    {
        public string enemyName;
        [Tooltip("Short description of the enemy in case we display it on UI a day")]
        public string description;
        public Sprite sprite;

        [Range(1,3)]
        public int healthMax = 1;
        
        [Tooltip("Number of actions the enemy can do in one turn")]
        [Range(1,3)]
        public int actionPerMove = 1;

        [Tooltip("The number of time the player needs to scare the enemy so it run away from the game")]
        [Range(1, 5)]
        public int bravery = 1;

        private void Reset()
        {
            enemyName = "Enemy";
            description = "**description**";
            healthMax = 1;
            actionPerMove = 1;
            bravery = 1;
        }
    }
}