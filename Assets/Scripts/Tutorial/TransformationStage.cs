using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GoldProject;
using Enemies;

public class TransformationStage : StageBase {

    private Player player;

    public TransformationStage(TextMeshProUGUI stateText, string stateDesc) : base(stateText,stateDesc) {

    }

    public override void OnStageBegin() {
        base.OnStageBegin();

        player = PlayerManager.Instance.Player;
        Debug.Log("transformation begin");
        player.CanTransform = true;
        player.Transform();

        Vector3 enemyPos = player.CurrentRoom.roomTransform.GetChild(player.CurrentRoom.roomTransform.childCount - 1).position;
        
        
        GameObject enemy = TutorialManager.Instantiate(TutorialManager.Instance.enemyPrefab, enemyPos, Quaternion.identity);
        enemy.GetComponent<EnemyBase>().enabled = false;
        enemy.transform.GetChild(3).gameObject.SetActive(false); // Disable enmy light 
    }

    public override void OnStageUpdate() {
        base.OnStageUpdate();
    }

    public override void OnStageFinish() => base.OnStageFinish();
    

}
