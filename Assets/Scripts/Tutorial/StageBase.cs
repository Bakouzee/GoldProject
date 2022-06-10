using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageBase : TutorialStage {

    public StageBase(Text stateText,string stateDesc, TutorialStage nextStage) : base(stateText,stateDesc, nextStage) {}

    protected override void OnStageBegin() {
        stateText.text = stateDesc;
    }


    protected override void OnStageUpdate() { }
    protected override void OnStageFinish() {
        stateText.text = "";
    }
}
