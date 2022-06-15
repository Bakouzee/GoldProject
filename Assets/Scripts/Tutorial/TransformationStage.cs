using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GoldProject;
public class TransformationStage : StageBase {

    private Player player;

    public TransformationStage(TextMeshProUGUI stateText, string stateDesc) : base(stateText,stateDesc) {

    }

    public override void OnStageBegin() {
        base.OnStageBegin();

        player = PlayerManager.Instance.Player;

        player.Transform();
    }

    public override void OnStageUpdate() {
        base.OnStageUpdate();
    }

    public override void OnStageFinish() => base.OnStageFinish();
    

}
