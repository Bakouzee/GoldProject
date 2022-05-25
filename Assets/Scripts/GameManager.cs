using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
   public enum DayState
    {
        DAY,
        NIGHT
    };

    public static DayState dayState = DayState.DAY;

    public void StartDay()
    {
        dayState = DayState.DAY;
        EnemyManager.enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy")); // can be changed by "FindGameObjectsOfType<>"
    }
    
    public void StartNight()
    {
        dayState = DayState.NIGHT;
        EnemyManager.enemies.Clear(); // Reset all enemies in the list
    }

    public void MoveAllEnemies(List<GameObject> enemies)
    {

    }
}
