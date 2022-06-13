using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GridSystem;
using GoldProject.Rooms;

public class VentStage : MovementStage {

    private VentManager ventManager;
    private Room beginRoom;

    private bool lastIsFinish;

    public VentStage(Vector2Int tileTargetPos, TextMeshProUGUI stateText, string stateDesc, TutorialStage nextStage) : base(tileTargetPos,stateText, stateDesc, nextStage) {
        this.tileTarget = GridManager.Instance.GetTileAtPosition(tileTargetPos).gameObject;
        this.tileTargetPos = tileTargetPos;
    }

    public override void OnStageBegin() {
        base.OnStageBegin();
        ventManager = GameObject.FindObjectOfType<VentManager>();
        beginRoom = PlayerManager.Instance.Player.CurrentRoom;

    }
    public override void OnStageUpdate() {
        base.OnStageUpdate();
        
        if(isFinish) {
            isFinish = false;
            lastIsFinish = true;
        }

        if(lastIsFinish && beginRoom != PlayerManager.Instance.Player.CurrentRoom)
            isFinish = true;
        
            

    }

    public override void OnStageFinish() => base.OnStageFinish();
    

}
