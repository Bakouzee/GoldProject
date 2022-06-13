using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapStage : StageBase {

    private GameObject trapsUI;

    public TrapStage(GameObject trapsUI,TextMeshProUGUI stateText, string stateDesc, TutorialStage nextStage) : base(stateText, stateDesc, nextStage) {
        this.trapsUI = trapsUI;
    }


    public override void OnStageBegin() {
        base.OnStageBegin();

        trapsUI.GetComponent<Animator>().enabled = true;
    }

    public override void OnStageUpdate() {
        base.OnStageUpdate();
    }

    public override void OnStageFinish() {
        base.OnStageFinish();

        trapsUI.GetComponent<Animator>().enabled = false;
    }
}
