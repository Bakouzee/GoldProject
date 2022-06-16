using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageBase : TutorialStage {

    public StageBase(TextMeshProUGUI stateText,List<string> stageDescs,string subDesc,int id) : base(stateText,stageDescs,subDesc,id) {}

    public override void OnStageBegin() => TutorialManager.Instance.StartCoroutine(TutorialManager.Instance.ShowStageText(0));
    


    public override void OnStageUpdate() {

        if (isFinish)
            OnStageFinish();
        
        
   }
    public override void OnStageFinish() {
        stateText.text = "";
        
        TutorialManager.Instance.displaySubDialog = false;
        
        nextStage?.OnStageBegin();

        if (nextStage != null)
            TutorialManager.Instance.currentStage = nextStage;
        else {
            TutorialManager.Instance.currentStage = null;
            TutorialManager.Instance.endMenu.SetActive(true);
        }
    }
}
