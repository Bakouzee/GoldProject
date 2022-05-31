using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEventsManager : SingletonBase<TrapEventsManager>
{
    //GetPath
    public void NoiseEvent()
    {
        Debug.Log("Noise");
    }
    public void KnightEvent()
    {
        Debug.Log("Knight");
    }
    public void PuppetEvent()
    {
        Debug.Log("Puppet");
    }
    public void MirrorEvent()
    {
        Debug.Log("Mirror");
    }
    public void PaintingEvent()
    {
        Debug.Log("Painting");
    }
}
