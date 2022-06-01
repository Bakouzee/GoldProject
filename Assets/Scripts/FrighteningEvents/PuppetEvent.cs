using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoldProject.FrighteningEvent;

public class PuppetEvent : FrighteningEventBase
{
    private void OnMouseDown()
    {
        Do();
    }
    protected override IEnumerator DoActionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("done");
    }

    protected override IEnumerator UndoActionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("undone");
    }
}
