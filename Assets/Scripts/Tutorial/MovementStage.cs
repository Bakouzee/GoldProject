using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementStage : StageBase {

    private Vector2Int tileTargetPos;

    public MovementStage(Vector2Int tileTargetPos,Text stateText,string stateDesc, TutorialStage nextStage) : base(stateText,stateDesc,nextStage) {
        this.tileTargetPos = tileTargetPos;
       
    }

    protected override void OnStageBegin() {}
    protected override void OnStageUpdate() {}
    protected override void OnStageFinish() {}


}
