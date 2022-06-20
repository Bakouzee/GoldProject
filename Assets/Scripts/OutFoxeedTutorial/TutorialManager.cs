using System.Collections;
using System.Collections.Generic;
using GoldProject.Rooms;
using GridSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace OutFoxeedTutorial
{
    public class TutorialManager : SingletonBase<TutorialManager> 
    {
        [SerializeField]
        private TextMeshProUGUI stateText;

        private Queue<OutFoxeedTutorial.TutorialState> states;
        private OutFoxeedTutorial.TutorialState currentState;

        public GameObject enemyPrefab;

        public GameObject endMenu;

        public GameObject[] directionalArrows = new GameObject[4];

        public bool displaySubDialog;


        [Space(10)]
        [SerializeField] private OutFoxeedTutorial.States.MovementState firstMovementState;
        [SerializeField] private OutFoxeedTutorial.States.MovementState secondMovementState;
        [SerializeField] private OutFoxeedTutorial.States.TrapState trapState;
        [SerializeField] private OutFoxeedTutorial.States.CurtainState curtainState;
        [SerializeField] private OutFoxeedTutorial.States.BonusesState bonusesState;
        [SerializeField] private OutFoxeedTutorial.States.VentState ventState;
        [SerializeField] private OutFoxeedTutorial.States.TransformationState transformationState;

        // 11;27
        private void Start()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null && !gameManager.enabled)
                gameManager.enabled = true;
        
            InitStages();
            GoToNextState();
             
            // curtain.GetComponent<Curtain>().SetOpened(true);
        }

        private void InitStages() {
            states = new Queue<TutorialState>();
            states.Enqueue(firstMovementState);
            states.Enqueue(secondMovementState);
            states.Enqueue(trapState);
            states.Enqueue(curtainState);
            states.Enqueue(bonusesState);
            states.Enqueue(ventState);
            states.Enqueue(transformationState);
        }
        public void Update() => currentState?.OnStateUpdate();

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

        private IEnumerator textCoroutine;
        private IEnumerator HandleTextCoroutine()
        {
            string currentText = currentState.CurrentText;
            stateText.text = "";
            for (int i = 0; i < currentText.Length; i++)
            {
                stateText.text += currentText[i];
                yield return new WaitForSecondsRealtime(0.03f);
            }

            yield return new WaitForSeconds(3.5f);
            textCoroutine = null;
            PassText();
        }
        private void PassText()
        {
            if (currentState.CurrentText == currentState.texts[^1])
                return;
            
            currentState.NextText();
            if (textCoroutine != null)
            {
                StopCoroutine(textCoroutine);
                textCoroutine = null;
            }

            textCoroutine = HandleTextCoroutine();
            StartCoroutine(textCoroutine);
        }
        
        public void GoToNextState()
        {
            if (currentState == null)
            {
                if (states.Count == 0)
                    return;
                currentState = states.Dequeue();
                currentState.OnStateEnter();

                textCoroutine = HandleTextCoroutine();
                StartCoroutine(textCoroutine);
                return;
            }
            
            // End state
            currentState.OnStateExit();
            
            if (states.Count > 0)
            {
                // Start new state
                currentState = states.Dequeue();
                currentState.OnStateEnter();
                
                // Stop and start new text
                if(textCoroutine != null)
                    StopCoroutine(textCoroutine);
                textCoroutine = HandleTextCoroutine();
                StartCoroutine(textCoroutine);
            }
            else
            {
                // Stop
                EndTuto();   
            }
        }

        private void EndTuto()
        {
            currentState = null;
            GameManager.SetPause(true);
            if(endMenu) endMenu.SetActive(true);
        }
    }
}