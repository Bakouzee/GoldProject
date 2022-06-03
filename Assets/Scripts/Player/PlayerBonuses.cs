using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoldProject
{
    public class PlayerBonuses : MonoBehaviour
    {
        [HideInInspector]
        public PlayerManager PlayerManager;

        public List<HealthRelatedBonus> bonuses;
        
        public int GetBonusesOfType(Bonus.Type type)
        {
            int result = 0;
            foreach (var bonus in bonuses)
            {
                if (!bonus.enabled)
                    continue;
                
                if (bonus.type == type)
                    result += bonus.value;
            }
            return result;
        }
        

        private void Start()
        {
            PlayerManager.PlayerHealth.OnHealthUpdated += (newHealth, healthMax) =>
            {
                float healthPercentage = newHealth / (float) healthMax;

                for (int i = 0; i < bonuses.Count; i++)
                    bonuses[i].enabled = healthPercentage <= bonuses[i].healthPercentageNeeded;
            };
        }
    }
}