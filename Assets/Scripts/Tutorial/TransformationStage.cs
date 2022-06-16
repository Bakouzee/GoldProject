using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GoldProject;
using Enemies;
using GoldProject.UI;
using Unity.VisualScripting;

public class TransformationStage : StageBase {

    private Player player;
    private GameObject enemy;

    private float timer;
    
    public TransformationStage(TextMeshProUGUI stateText,List<string> stageDescs,string subDesc,int id) : base(stateText,stageDescs,subDesc,id) {

    }

    public override void OnStageBegin() {
        base.OnStageBegin();

        player = PlayerManager.Instance.Player;
        player.CanTransform = true;
        player.Transform();
        GameObject.FindObjectOfType<TransformationIndicator>().SetIndicator(true);

        Vector3 enemyPos = player.CurrentRoom.roomTransform.GetChild(player.CurrentRoom.roomTransform.childCount - 1).position;
        
        
        enemy = TutorialManager.Instantiate(TutorialManager.Instance.enemyPrefab, enemyPos, Quaternion.identity);
        enemy.GetComponent<EnemyBase>().enabled = false;
        enemy.transform.GetChild(3).gameObject.SetActive(false); // Disable enmy light 
        
        
    }

    public override void OnStageUpdate() {
        base.OnStageUpdate();
        
       isFinish = enemy.GetComponent<EnemyBase>().health.currentHealth <= 0;
    }

    public override void OnStageFinish() {
        base.OnStageFinish();
        
        GameObject.FindObjectOfType<TransformationIndicator>().SetIndicator(false);
    }


}
