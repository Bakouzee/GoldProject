using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoldProject.Rooms;

public class TutorialManager : SingletonBase<TutorialManager> {

    [SerializeField]
    private TextMeshProUGUI stateText;

    private List<StageBase> stages = new List<StageBase>();
    public TutorialStage currentStage;


    [Header("Movement Stage")]
    [SerializeField]
    private Vector2Int movementTile;

    [Header("Traps Stage")]
    [SerializeField]
    private GameObject trapsButton;

    [Header("Curtain Stage")]
    [SerializeField]
    private Vector2Int curtainTile;
    [SerializeField]
    private GameObject curtain;

    private void Start() {
        InitStages();


        currentStage = stages[0];
        stages[0].OnStageBegin();

        curtain.GetComponent<Curtain>().SetOpened(true);
    }

    private void InitStages() {
        MovementStage movementStage = new MovementStage(movementTile,stateText,"Déplacez-vous vers la case qui clignote",null);
        TrapStage trapStage = new TrapStage(trapsButton, stateText, "Ouvrez le menu pour activer les pièges", null);
        CurtainStage curtainStage = new CurtainStage(curtainTile, stateText, "Déplacez vous vers la fenêtre", null);

        movementStage.nextStage = trapStage;
        trapStage.nextStage = curtainStage;

        stages.Add(movementStage);
        stages.Add(trapStage);
        stages.Add(curtainStage);

    }
    public void Update() => currentStage?.OnStageUpdate();
    
 
}
