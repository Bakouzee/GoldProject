using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : SingletonBase<PlayerManager>
{
    public Canvas map;
    private bool mapSeen = false;

    private void Start()
    {
        map.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mapSeen = !mapSeen;
            ShowMap(mapSeen);
        }
    }

    private void ShowMap(bool canSeeMap)
    {
        if (map != null)
        {
            if (canSeeMap)
            {
                map.enabled = true;
            } else
            {
                map.enabled = false;
            }
        }
    }
}
