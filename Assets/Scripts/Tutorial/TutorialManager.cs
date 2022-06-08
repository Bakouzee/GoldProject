using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState
{
    NONE,
    TRAPS,
    CURTAIN,
    VENT,
    TRANSFORMATION,
}

public class TutorialManager : CoroutineSystem {

    [SerializeField]
    private TutorialState actualState;

    private TutorialState lastState;

    private void Start() {
        SwitchState(TutorialState.TRAPS);
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
            PlayerManager.Instance.ShowMap();
            Debug.Log("show my mâp");
         });
    }
}
