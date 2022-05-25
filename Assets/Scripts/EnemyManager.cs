using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonBase<EnemyManager>
{
    public static List<GameObject> enemies = new List<GameObject>();
}
