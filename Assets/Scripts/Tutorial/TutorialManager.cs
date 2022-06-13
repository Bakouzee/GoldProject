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

    [Header("Vent Stage")]
    [SerializeField]
    private Vector2Int ventTile;

    // 11;27
    private void Start() {
        InitStages();


        currentStage = stages[0];
        stages[0].OnStageBegin();

        curtain.GetComponent<Curtain>().SetOpened(true);
    }

    private void InitStages() {
        MovementStage movementStage = new MovementStage(movementTile,stateText,"Déplacez-vous vers la case qui clignote",null);
        TrapStage trapStage = new TrapStage(trapsButton, stateText, "Ouvrez le menu pour activer les pièges", null);
        CurtainStage curtainStage = new CurtainStage(curtain.GetComponent<Curtain>(),curtainTile, stateText, "Déplacez vous vers la fenêtre", null);
        VentStage ventStage = new VentStage(ventTile, stateText, "Déplacez-vous vers la vent", null);

        movementStage.nextStage = trapStage;
        trapStage.nextStage = curtainStage;
        curtainStage.nextStage = ventStage;

        stages.Add(movementStage);
        stages.Add(trapStage);
        stages.Add(curtainStage);
        stages.Add(ventStage);
    }
    public void Update() => currentStage?.OnStageUpdate();
    
 
}
