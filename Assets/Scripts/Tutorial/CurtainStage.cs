using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GridSystem;

public class CurtainStage : MovementStage
{

    public CurtainStage(Vector2Int tileTargetPos, TextMeshProUGUI stateText, string stateDesc, TutorialStage nextStage) : base(tileTargetPos,stateText, stateDesc, nextStage)
    {
        this.tileTarget = GridManager.Instance.GetTileAtPosition(tileTargetPos).gameObject;
        this.tileTargetPos = tileTargetPos;
    }

    public override void OnStageBegin()
    {
        base.OnStageBegin();

    }

    public override void OnStageUpdate()
    {

        base.OnStageUpdate();

        Debug.Log("curtain update");
        // En dessous de 50% => va plus vite 

        if(isFinish)
        {
            isFinish = false;
            Debug.Log("finished");
        }

        
    }

    public override void OnStageFinish() => base.OnStageFinish();
    
}
