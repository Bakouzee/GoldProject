using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SplashArtManager : SingletonBase<SplashArtManager>
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
}
