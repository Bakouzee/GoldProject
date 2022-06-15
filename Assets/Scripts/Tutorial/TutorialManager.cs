using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoldProject.Rooms;
using UnityEngine.SceneManagement;

public class TutorialManager : SingletonBase<TutorialManager> {

    [SerializeField]
    private TextMeshProUGUI stateText;

    private List<StageBase> stages = new List<StageBase>();
    public TutorialStage currentStage;

    public GameObject endMenu;


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

    [Header("Transformation Stage")] 
    public GameObject enemyPrefab;
    // 11;27
    private void Start()
    {

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null && !gameManager.enabled)
            gameManager.enabled = true;
        
        InitStages();


        currentStage = stages[0];
        stages[0].OnStageBegin();

        curtain.GetComponent<Curtain>().SetOpened(true);
    }

    private void InitStages() {
        MovementStage movementStage = new MovementStage(movementTile,stateText,"Déplacez-vous vers la case qui clignote");
        TrapStage trapStage = new TrapStage(trapsButton, stateText, "Ouvrez le menu pour activer les pièges");
        CurtainStage curtainStage = new CurtainStage(curtain.GetComponent<Curtain>(),curtainTile, stateText, "Déplacez vous vers la fenêtre");
        VentStage ventStage = new VentStage(ventTile, stateText, "Déplacez-vous vers la vent");
        TransformationStage transformationStage = new TransformationStage(stateText, "Tuez l'ennemi devant vous");
        
        movementStage.nextStage = trapStage;
        trapStage.nextStage = curtainStage;
        curtainStage.nextStage = ventStage;
        ventStage.nextStage = transformationStage;
        
        stages.Add(movementStage);
        stages.Add(trapStage);
        stages.Add(curtainStage);
        stages.Add(ventStage);
        stages.Add(transformationStage);
    }
    public void Update() => currentStage?.OnStageUpdate();

    public void MenuButton() => SceneManager.LoadScene("Menu");
    public void FinishButton() => SceneManager.LoadScene("Victoria_LD");
    
    

}
