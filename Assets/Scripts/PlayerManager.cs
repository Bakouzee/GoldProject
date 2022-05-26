using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : SingletonBase<PlayerManager>
{
    public GameObject miniMap;
    public GameObject mainCam;
    public static bool mapSeen = false;

    private void Start()
    {
        miniMap.SetActive(false);
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
        if (miniMap != null)
        {
            if (canSeeMap)
            {
                miniMap.SetActive(true);
                mainCam.SetActive(false);
            } else
            {
                mainCam.SetActive(true);
                miniMap.SetActive(false);
            }
        }
    }
}
