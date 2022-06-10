using System.Linq;
using GridSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using Enemies.States;
using Enemies;
using System.Collections.Generic;

public class GoToState : EnemyFollowedState
{

    private Vector2Int goTo;

    public GoToState(EnemyBase enemy, Vector2Int goTo, EnemyBaseState nextState) : base(enemy, nextState)
    {
        this.goTo = goTo;
    }

    public override IEnumerator OnStateEnter()
    {
        DefinePath(goTo);
        yield return null;
    }

    public override void DoAction()
    {

        gridController.Move(directions.Dequeue(), animator);

        if (directions.Count == 0)
            GoToNextState();
    }
}
