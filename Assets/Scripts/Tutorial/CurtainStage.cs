using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GridSystem;
using GoldProject.Rooms;

public class CurtainStage : MovementStage
{
    private Curtain curtain;

    public CurtainStage(Curtain curtain,Vector2Int tileTargetPos, TextMeshProUGUI stateText,List<string> stageDescs,string subStage,int id) : base(tileTargetPos,stateText,stageDescs, subStage,id)
    {
        this.curtain = curtain;
        this.tileTarget = GridManager.Instance.GetTileAtPosition(tileTargetPos).gameObject;
        this.tileTargetPos = tileTargetPos;
    }

    public override void OnStageBegin() => base.OnStageBegin();

    public override void OnStageUpdate() {
        base.OnStageUpdate();

        if(isFinish) {
            isFinish = false;
            if(PlayerManager.Instance.PlayerHealth.CurrentHealth <= PlayerManager.Instance.PlayerHealth.healthMax / 2) {
                isFinish = true;
                curtain.SetOpened(false);
            }
        }   
    }

    public override void OnStageFinish() => base.OnStageFinish();
    
}
