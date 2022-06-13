using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridSystem;
using TMPro;
using GoldProject;

public class MovementStage : StageBase {

    private GameObject tileTarget;
    private Player player;
    private Vector2Int tileTargetPos;

    public MovementStage(Vector2Int tileTargetPos, TextMeshProUGUI stateText,string stateDesc, TutorialStage nextStage) : base(stateText,stateDesc,nextStage) {
        this.tileTarget = GridManager.Instance.GetTileAtPosition(tileTargetPos).gameObject;
        this.tileTargetPos = tileTargetPos;
    }

    public override void OnStageBegin() {
        base.OnStageBegin();

        player = PlayerManager.Instance.Player;
        tileTarget.transform.GetChild(2).gameObject.SetActive(true);
    }

    public override void OnStageUpdate() {
        base.OnStageUpdate();
        Vector2Int playerPos = GridManager.Instance.GetGridPosition(player.transform.position);

        isFinish = playerPos == tileTargetPos;
    }

    public override void OnStageFinish() {
        base.OnStageFinish();

        tileTarget.transform.GetChild(2).gameObject.SetActive(false); 
    }


}
