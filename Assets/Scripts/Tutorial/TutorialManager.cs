using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GoldProject.Rooms;
using GridSystem;
using UnityEngine.SceneManagement;

public class TutorialManager : SingletonBase<TutorialManager> {

    
    /**
     *
     * Faire un systeme de sub text ou ya plusieurs textes en mm temps 
     * Faire les Zooms de caméra sur les élements pour les expliquer 
     * 
     */
    
    [SerializeField]
    private TextMeshProUGUI stateText;

    private List<StageBase> stages = new List<StageBase>();
    public TutorialStage currentStage;

    public GameObject enemyPrefab;

    public GameObject endMenu;

    public GameObject[] directionalArrows = new GameObject[4];

    public bool displaySubDialog;


    [Header("Movement Arrow Stage")]
    [SerializeField]
    private Vector2Int movementTileWithArrow;
    [SerializeField] 
    private List<string> movementArrowStageDescs;
    
    
    [Header("Movement Stage")]
    [SerializeField]
    private Vector2Int movementTile;
    [SerializeField] 
    private List<string> movementStageDescs;
    

    [Header("Traps Stage")]
    [SerializeField]
    private GameObject trapsButton;
    [SerializeField] 
    private List<string> trapsStageDescs;
    [SerializeField] 
    private string trapsSubText;

    [Header("Curtain Stage")]
    [SerializeField]
    private Vector2Int curtainTile;
    [SerializeField]
    private GameObject curtain;
    [SerializeField] 
    private List<string> curtainStageDescs;

    [Header("Vent Stage")]
    [SerializeField]
    private Vector2Int ventTile;
    
    [SerializeField] 
    private List<string> ventStageDescs;
    [SerializeField] 
    private string ventSubText;

    [Header("Transformation Stage")]
    [SerializeField] 
    private List<string> transStageDescs;
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
        StartCoroutine(ShowStageText(0));
    }

    private void InitStages() {
        MovementStage movementArrowStage = new MovementStage(movementTileWithArrow,stateText,movementArrowStageDescs,"",0);
        MovementStage movementStage = new MovementStage(movementTile,stateText,movementStageDescs,"",1);
        TrapStage trapStage = new TrapStage(trapsButton, stateText, trapsStageDescs,trapsSubText,2);
        CurtainStage curtainStage = new CurtainStage(curtain.GetComponent<Curtain>(),curtainTile, stateText, curtainStageDescs,"",3);
        VentStage ventStage = new VentStage(ventTile, stateText, ventStageDescs,ventSubText,4);
        TransformationStage transformationStage = new TransformationStage(stateText, transStageDescs,"",5);
        
        movementArrowStage.nextStage = movementStage;
        movementStage.nextStage = trapStage;
        trapStage.nextStage = curtainStage;
        curtainStage.nextStage = ventStage;
        ventStage.nextStage = transformationStage;
        
        stages.Add(movementArrowStage);
        stages.Add(movementStage);
        stages.Add(trapStage);
        stages.Add(curtainStage);
        stages.Add(ventStage);
        stages.Add(transformationStage);
    }
    public void Update() => currentStage?.OnStageUpdate();

    public void MenuButton() => SceneManager.LoadScene("Menu");
    public void FinishButton() => SceneManager.LoadScene("Victoria_LD");


    public GameObject FindArrowByDirections(Direction dir) {
        switch (dir.ToString()) {
            case "Up":
                return directionalArrows[0];
            
            case "Down":
                return directionalArrows[1];
            
            case "Left":
                return directionalArrows[2];
            
            case "Right":
                return directionalArrows[3];
        }

        return null;
    }

    public IEnumerator ShowStageText(int index)
    {
        if(index >= currentStage.stageDescs.Count)
            yield break;
        
        for(int i = 0;i<currentStage.stageDescs[index].Length;i++) {
            yield return new WaitForSeconds(0.03f);
            stateText.text = currentStage.stageDescs[index][..i];
        }

        if (index + 1 < currentStage.stageDescs.Count) {
            yield return new WaitForSeconds(2f);
            StartCoroutine(ShowStageText(index + 1));
        }

    }
    
    public IEnumerator ShowSubText()
    {
        for(int i = 0;i<currentStage.subDesc.Length;i++) {
            yield return new WaitForSeconds(0.03f);
            stateText.text = currentStage.subDesc.Substring(0,i);
        }


    }

}
