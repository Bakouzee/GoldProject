using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridSystem;
using TMPro;
using GoldProject;

public class MovementStage : StageBase {

    protected GameObject tileTarget;
    private Player player;
    protected Vector2Int tileTargetPos;
    
    private List<Direction> pathDirections;
    private Direction lastDirection;

    public MovementStage(Vector2Int tileTargetPos, TextMeshProUGUI stateText,List<string> stageDescs,string subDesc,int id) : base(stateText,stageDescs,subDesc,id) {
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
        Debug.Log("update");
        pathDirections = GridManager.Instance.GetPath(GridManager.Instance.GetGridPosition(player.transform.position),tileTargetPos);
        
        if (pathDirections.Count > 0) {
            SwitchArrowState(true, pathDirections[0]);
            SwitchArrowState(!(lastDirection != null && lastDirection.Value != pathDirections[0].Value), lastDirection);
        }

        isFinish = playerPos == tileTargetPos;

        lastDirection = pathDirections.Count > 0 ? pathDirections[0] : null;
    }

    public override void OnStageFinish() {
        base.OnStageFinish();

        tileTarget.transform.GetChild(2).gameObject.SetActive(false); 
    }

    private void SwitchArrowState(bool state,Direction dir) {
        if (dir == null)
            return;
        
        TutorialManager.Instance.FindArrowByDirections(dir).GetComponent<Animator>().enabled = state;
        TutorialManager.Instance.FindArrowByDirections(dir).transform.GetChild(0).gameObject.SetActive(state);
    }
    
    

}
