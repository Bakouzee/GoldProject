using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapStage : StageBase {

    private GameObject trapsUI;

    public TrapStage(GameObject trapsUI,TextMeshProUGUI stateText, string stateDesc) : base(stateText, stateDesc) {
        this.trapsUI = trapsUI;
    }


    public override void OnStageBegin() {
        base.OnStageBegin();

        trapsUI.GetComponent<Animator>().enabled = true;
    }

    public override void OnStageUpdate() {
        base.OnStageUpdate();

        if (PlayerManager.mapSeen) {
            stateText.text = "Appuyez sur un piège pour l'activer";
            foreach(NoiseEvent noiseEvent in GameObject.FindObjectsOfType<NoiseEvent>()) {
                if(noiseEvent.IsTriggered) {
                    PlayerManager.Instance.ShowMap();
                    isFinish = true;
                    break;
                }
            }

        }
    }

    public override void OnStageFinish() {
        base.OnStageFinish();

        trapsUI.GetComponent<Animator>().enabled = false;
    }
}
