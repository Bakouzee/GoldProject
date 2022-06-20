using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;
using Enemies;
using AudioController;

public class KnightEvent : FrighteningEventBase
{
    private EnemyBase enemyToScare;
    private List<EnemyBase> afraidEnemies = new List<EnemyBase>();
    [SerializeField]
    private SpriteRenderer[] srMap;
    [SerializeField] private SpriteRenderer minimapCircle;
    [SerializeField]
    private Animator animKnight;

    public List<Direction> directionKnight = new List<Direction>();
    private Vector2Int knightPos;

    public int NumberOfTurnTheKnightCanMove = 5;
    private int index = 0;

    private bool isReseting;

    private void Start()
    {
        // srMap[1] -> child srMap[0] -> this
        // need to be in range quand la map est désactivé
        if(srMap == null)
            srMap = GetComponentsInChildren<SpriteRenderer>();

        gridController = new GridController(transform);

        knightPos = GridManager.Instance.GetGridPosition(transform.position);

        if (minimapCircle)
            minimapCircle.transform.localScale = Vector3.one * distanceToBeScared * 2f;
        
        if(!animKnight)
            animKnight = GetComponentInChildren<Animator>();

        isReseting = false;

        PlayerManager.Instance.onShowMap += () => needToBeInRange = PlayerManager.mapSeen;
    }

    public override bool TryInteract()
    {
        if (!base.TryInteract())
            return false;
        
        // Can't interact if it is rearming
        if (isReseting)
            return false;
        
        // normally have to activate the trap AND WHEN an enemy is at his range or in the room
        // the armor will move to him
        
        if(IsTriggered)
            Undo();
        else Do();
        return true;
    }


    //from a script to tell the knight where to move
    protected override IEnumerator DoActionCoroutine()
    {
        if (CurrentRoom.enemies.Count == 0)
        {
            // Show the trap didn't work
            foreach (var spriteRenderer in srMap)
                spriteRenderer.color = Color.red;
            // srMap[1].color = Color.red;
            isReseting = false;
            yield break;
        }

        EnemyManager.knights.Add(this);

        //get the closest enemy pos so the knight will move to the enemy --> HAVE TO MAKE THE KNIGHT MOVE AT
        // THE SAME TIME OF THE ENEMIES

        // The trap worked
        foreach (var spriteRenderer in srMap)
            spriteRenderer.color = Color.green;
        // srMap[1].color = Color.green;

        enemyToScare = CurrentRoom.GetClosestEnemy(transform.position);
        afraidEnemies.Clear();

        //Get the path to do
        directionKnight = GridManager.Instance.GetPath(knightPos, enemyToScare.GridController.gridPosition);

        // If the enemy is directly next to the trap -> he will be scared !
        if(directionKnight.Count < distanceToBeScared)
        {
            enemyToScare.GetAfraid(transform);
            afraidEnemies.Add(enemyToScare);
        }
        yield return new WaitForSeconds(1f);

    }

    protected override IEnumerator UndoActionCoroutine()
    {
        // Reset the start position of the knight
        // gridController.SetPosition(knightPos);

        index = 0;
        directionKnight.Clear();

        Vector2Int actualKnightPos = GridManager.Instance.GetGridPosition(transform.position);
        directionKnight = GridManager.Instance.GetPath(actualKnightPos, knightPos);

        isReseting = CurrentRoom.enemies.Count > 0;

        if (GridManager.Instance.GetGridPosition(transform.position) == knightPos)
        {
            isReseting = false;
        }

        yield return new WaitForSeconds(1f);

        Color32 readyColor = new Color32(166, 79, 0, 255);
        foreach (var spriteRenderer in srMap)
            spriteRenderer.color = readyColor;
        // srMap[1].color = readyColor;
    }

    public void MoveKnight()
    {
        //Debug.Log("index : " + index);
        if (index < NumberOfTurnTheKnightCanMove && !isReseting)
        {
            // Check if the knight is at distance to scare the enemy
            // List<Direction> refreshedDistance = GridManager.Instance.GetPath(knightPos, enemyToScare.GridController.gridPosition);
            // if (refreshedDistance.Count < distanceToBeScared)
            // {
            //     enemyToScare.GetAfraid(transform);
            // }

            // Afray close enemies
            foreach (var enemyInRoom in CurrentRoom.enemies)
            {
                // If too far, continue
                if (gridController.gridManager.GetManhattanDistance(enemyInRoom.gridController.gridPosition,
                    gridController.gridPosition) > distanceToBeScared) 
                    continue;
                
                // If already afraid, continue
                if (afraidEnemies.Contains(enemyInRoom))
                    continue;
                
                // Frigten enemy and add to the list
                enemyInRoom.GetAfraid(transform);
                afraidEnemies.Add(enemyInRoom);
            }

            // Move the knight until he can't
            if (index < directionKnight.Count)
            {
                if (gridController != null)
                {
                    gridController.Move(directionKnight[index], animKnight);
                    AudioManager.Instance.PlayScarySound(ScaryAudioTracks.Sc_StatueWalk);
                    index++;
                }
            }

            if (GridManager.Instance.GetGridPosition(transform.position) == knightPos)
            {
                if (index == directionKnight.Count)
                {
                    EnemyManager.knights.Remove(this);
                    index = 0;
                }
            }
        }

        if (isReseting && GameManager.dayState == GameManager.DayState.NIGHT)
        {
            if (index < directionKnight.Count)
            {
                if (gridController != null)
                {
                    gridController.Move(directionKnight[index], animKnight);
                    index++;
                }
            }

            if (GridManager.Instance.GetGridPosition(transform.position) == knightPos)
            {
                animKnight.SetTrigger("Down");
                if (index == directionKnight.Count)
                {
                    EnemyManager.knights.Remove(this);
                    isReseting = false;
                    index = 0;
                }
            }
        }
    }

    private void OnDestroy()
    {
        EnemyManager.knights.Remove(this);
    }
}