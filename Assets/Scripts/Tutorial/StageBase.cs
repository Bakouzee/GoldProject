using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageBase : TutorialStage {

    public StageBase(TextMeshProUGUI stateText,string stateDesc, TutorialStage nextStage) : base(stateText,stateDesc, nextStage) {}

    public override void OnStageBegin() {
        stateText.text = stateDesc;
    }


    public override void OnStageUpdate() {

        if (isFinish)
            OnStageFinish();
        
        
   }
    public override void OnStageFinish() {
        stateText.text = "";

        nextStage?.OnStageBegin();

        if (nextStage != null)
            TutorialManager.Instance.currentStage = nextStage;
        else {
            TutorialManager.Instance.currentStage = null;
            Debug.Log("end of tuto");
        }
    }
}
