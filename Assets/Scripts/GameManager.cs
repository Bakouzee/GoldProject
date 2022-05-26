using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
   public enum DayState
    {
        DAY,
        NIGHT
    };

    public Camera minimapCam;
    
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

    public void MoveAllEnemies()
    {
        foreach (var enemy in EnemyManager.enemies)
        {
            // Do something
        }
    }

    #region UI Methods

    public void ActivateTrap(Button trapButton)
    {
        // Have to reset if the player reactivates the trap
        ColorBlock colorTrapActivated = new ColorBlock();
        colorTrapActivated.disabledColor = Color.green;
        colorTrapActivated.colorMultiplier = 1;
        trapButton.interactable = false;
        trapButton.colors = colorTrapActivated;
        Debug.Log("Trap Activated");
    }

    #endregion
}
