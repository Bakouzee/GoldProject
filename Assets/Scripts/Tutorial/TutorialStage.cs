using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TutorialStage {

    public TutorialStage nextStage;
    public List<string> stageDescs;
    public string subDesc;
    protected TextMeshProUGUI stateText;
    protected bool isFinish;
    public int id;

    public TutorialStage(TextMeshProUGUI stateText,List<string> stageDescs,string subDesc,int id)
    {
        this.stateText = stateText;
        this.stageDescs = stageDescs;
        this.subDesc = subDesc;
        this.id = id;
    }
    
    public virtual void OnStageBegin() {}
    public virtual void OnStageUpdate() {}
    public virtual void OnStageFinish() {}
}
