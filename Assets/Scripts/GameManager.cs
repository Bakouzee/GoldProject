using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
    RawImage rawimg;
   public enum DayState
    {
        DAY,
        NIGHT
    };

    public GameObject trap;
    public GameObject trapButton;
    public Camera minimapCam;
    private RectTransform rectT;
    public static DayState dayState = DayState.DAY;

    private void Update()
    {
       /* if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectT, Input.mousePosition, null, out Vector2 localPoint);
            *//*localPoint.y = (rectT.rect.yMin*//*
            Debug.Log(localPoint);
        }*/
    }

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
