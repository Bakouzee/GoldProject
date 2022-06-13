using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridSystem;

public class MovementStage : StageBase {

    private GameObject tileTarget;

    public MovementStage(Vector2Int tileTargetPos,Text stateText,string stateDesc, TutorialStage nextStage) : base(stateText,stateDesc,nextStage) {
        this.tileTarget = GridManager.Instance.GetTileAtPosition(tileTargetPos).gameObject;
       
    }

    protected override void OnStageBegin() {
        base.OnStageBegin();

        tileTarget.transform.GetChild(2).gameObject.SetActive(true);
    }

    protected override void OnStageUpdate() {
    
        
    }

    protected override void OnStageFinish() {
        base.OnStageFinish();

        tileTarget.transform.GetChild(2).gameObject.SetActive(false); 
    }


}
