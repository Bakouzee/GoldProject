using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;
using GridSystem;
using AudioController;
using Enemies;

public class NoiseEvent : FrighteningEventBase
{
    private Animator anim;
    private SpriteRenderer sr;
    private SpriteRenderer[] srMap;

    public string animationTrigger;

    public bool canBeSeen;

    private void Start()
    {
        anim = transform.GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();
        sr.enabled = canBeSeen;

        srMap = GetComponentsInChildren<SpriteRenderer>();

        PlayerManager.Instance.onShowMap += () => needToBeInRange = PlayerManager.mapSeen;
    }

    public void MakeScarySound(ScaryAudioTracks audioToPlay)
    {
        AudioManager.Instance.PlayScarySound(audioToPlay);
    }

    public override bool TryInteract()
    {
        if (!base.TryInteract())
            return false;
        
        // normally have to activate the trap AND WHEN an enemy is at his range or in the room
        // the armor will move to him
        Do();

        return true;
    }

    // Check if enemy is in range to activate the animation
    protected override IEnumerator DoActionCoroutine()
    {
        if (CurrentRoom.enemies.Count == 0)
        {
            // show the trap didn't work
            srMap[1].color = Color.red;
            anim.SetBool(animationTrigger, true);
            sr.enabled = true;
            yield break;
        }

        foreach(EnemyBase enemy in CurrentRoom.enemies)
        {
            int directionBetweenTrapAndEnemy = GridManager.Instance.GetManhattanDistance(transform.position, enemy.transform.position);

            // If enemies are directly next to the trap -> they will be scared !
            if(directionBetweenTrapAndEnemy < distanceToBeScared)
            {
                enemy.GetAfraid(transform);
            }
        }

        //Launch animation
        srMap[1].color = Color.green;
        sr.enabled = true;
        anim.SetBool(animationTrigger, true);

        yield return new WaitForSeconds(1f);
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        Color32 readyColor = new Color32(166, 79, 0, 255);
        srMap[1].color = readyColor;
        // Reset animation
        anim.SetBool(animationTrigger, false);
        yield return new WaitForSeconds(1f);
        sr.enabled = canBeSeen;
    }
}
