using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialStage {

    protected TutorialStage nextStage;
    protected string stateDesc;
    protected Text stateText;

    public TutorialStage(Text stateText,string stateDesc, TutorialStage nextStage)
    {
        this.stateText = stateText;
        this.stateDesc = stateDesc;
        this.nextStage = nextStage;
    }
    protected virtual void OnStageBegin() {}
    protected virtual void OnStageUpdate() {}
    protected virtual void OnStageFinish() {}
}
