using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialState
{
    NONE,
    MOVEMENT,
    TRAPS,
    CURTAIN,
    VENT,
    TRANSFORMATION,
}

public class TutorialManager : CoroutineSystem {

    [SerializeField]
    private TutorialState actualState;
    [SerializeField]
    private Text stateText;
    [SerializeField]
    private Vector2Int tileMovement;

    private TutorialState lastState;

    private void Start() {
        SwitchState(TutorialState.MOVEMENT);
        lastState = actualState;
    }

    private void Update() {

        if (actualState != lastState)
            SwitchState(actualState);

        lastState = actualState;
    }

    private void SwitchState(TutorialState newState) {
        actualState = newState;
        Debug.Log("newState: " + newState);
        switch(newState) {
            case TutorialState.TRAPS:
                TrapsState();
                break;
        }

       
    }

    private void TrapsState() {
        RunDelayed(2f, () => {
            
         });
    }
}
