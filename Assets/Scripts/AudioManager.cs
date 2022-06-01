using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonBase<AudioManager>
{
    [Header("ScarySounds")]
    public List<AudioClip> scaryClipList = new List<AudioClip>();



}
