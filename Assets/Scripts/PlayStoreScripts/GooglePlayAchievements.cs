// <copyright file="GPGSIds.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

///
/// This file is automatically generated DO NOT EDIT!
///
/// These are the constants defined in the Play Games Console for Game Services
/// Resources.
///

using Enemies;
using GoldProject.FrighteningEvent;
using GoldProject.Rooms;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayStoreScripts
{
    /// Static class used to register achievements ID and unlock achievement
    public static class GooglePlayAchievements
    {
        #region Achievements
        /// <summary>Frighten an enemy</summary>
        public const string achievement_terror = "CgkI06WkmfgFEAIQAw"; // <GPGSID>
        
        /// <summary>Frighten/Kill a chief</summary>
        public const string achievement_first_step = "CgkI06WkmfgFEAIQBA"; // <GPGSID>
        
        /// <summary>Use all traps on the map</summary>
        public const string achievement_trap_master = "CgkI06WkmfgFEAIQBQ"; // <GPGSID>
        
        /// <summary>Kill every enemy during the night</summary>
        public const string achievement_serial_killer = "CgkI06WkmfgFEAIQBg"; // <GPGSID>
        
        /// <summary>Don't kill any enemy during 3 days</summary>
        public const string achievement_friendly_vampire = "CgkI06WkmfgFEAIQAQ"; // <GPGSID>
        public const int friendly_vampire_days_needed = 3;
        private static int friendly_vampire_day_counter = 0;
        
        /// <summary>Survive 5 days</summary>
        public const string achievement_survivor = "CgkI06WkmfgFEAIQBw"; // <GPGSID>
        public const int survivor_day_needed = 5;
        
        /// <summary>Die X times achievement</summary>
        public const string achievement_never_give_up = "CgkI06WkmfgFEAIQCA"; // <GPGSID>
        public const int never_give_up_death_needed = 5; // TODO: change this to 20
        #endregion

        public static void Unlock(string achievementId)
        {
            if (!PlayServices.usePlayServices)
                return;

            Social.ReportProgress(achievementId, 100.0d, success =>
            {
                // Handle success or not
            });
            
        }

        public static void Init()
        {
            // First step achievement
            EnemyManager.OnEnemyDisappeared += enemy =>
            {
                if (enemy.chief)
                    Unlock(achievement_first_step);
            };

            // Terror achievement
            EnemyManager.OnEnemyStartLeaving += enemy =>
            {
                Unlock(achievement_terror);
            };
            
            // Trap Master
            FrighteningEventBase.OnFrighteningEventTriggered += triggeredEvent =>
            {
                RoomsManager roomsManager = RoomsManager.Instance;
                if (roomsManager == null)
                    return;
                
                // If no frigthening event isn't triggered -> unlock achievement
                foreach (var room in roomsManager.Rooms)
                {
                    foreach (var frighteningEvent in room.frighteningEvents)
                    {
                        if (!frighteningEvent.IsTriggered)
                            return;
                    }
                }
                Unlock(achievement_trap_master);
            };

            // Serial killer
            EnemyManager.OnEnemyKilled += enemy =>
            {
                if(EnemyManager.enemiesCount == 0)
                    Unlock(achievement_serial_killer);
            };
            
            // Friendly vampire
            GameManager.Instance.OnDayChanged = currentDay => // We increment the counter of day without killing an enemy
                                                              // and unlock achievement if needed
            {
                friendly_vampire_day_counter++;
                if (friendly_vampire_days_needed <= friendly_vampire_day_counter)
                {
                    Unlock(achievement_friendly_vampire);
                }
            };
            EnemyManager.OnEnemyKilled += enemy => friendly_vampire_day_counter = 0; // Reset counter when killing an enemy
            
            // Survivor
            GameManager.Instance.OnDayChanged += currentDay =>
            {
                if (currentDay > survivor_day_needed) Unlock(achievement_survivor);
            };
            
            // Never give up 
            PlayerManager.Instance.PlayerHealth.OnHealthUpdated += (currentHealth, healthMax) =>
            {
                if (currentHealth <= 0)
                {
                    int newDeathCount = PlayerPrefs.GetInt("DeathCount", 0) + 1;
                    PlayerPrefs.SetInt("DeathCount", newDeathCount);
                    
                    if(newDeathCount >= never_give_up_death_needed)
                        Unlock(achievement_never_give_up);
                }
            };
        }
    }
}