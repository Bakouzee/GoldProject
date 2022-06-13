using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : SingletonBase<TutorialManager> {

    [SerializeField]
    private TextMeshProUGUI stateText;

    private List<StageBase> stages = new List<StageBase>();
    public TutorialStage currentStage;

    [Header("Movement Stage")]
    [SerializeField]
    private Vector2Int movementTile;

    private void Start() {
        InitStages();


        currentStage = stages[0];
        stages[0].OnStageBegin();
    }

    private void InitStages() {
        MovementStage movementStage = new MovementStage(movementTile,stateText,"Déplacez-vous vers la case qui clignote",null);

        stages.Add(movementStage);

    }
    public void Update() => currentStage?.OnStageUpdate();
    
 
}
