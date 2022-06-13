using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TutorialStage {

    protected TutorialStage nextStage;
    protected string stateDesc;
    protected TextMeshProUGUI stateText;
    protected bool isFinish;

    public TutorialStage(TextMeshProUGUI stateText,string stateDesc, TutorialStage nextStage)
    {
        this.stateText = stateText;
        this.stateDesc = stateDesc;
        this.nextStage = nextStage;
    }
    public virtual void OnStageBegin() {}
    public virtual void OnStageUpdate() {}
    public virtual void OnStageFinish() {}
}
