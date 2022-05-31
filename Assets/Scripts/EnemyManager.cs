using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemyManager : SingletonBase<EnemyManager>
{
    public static List<EnemyBase> enemies = new List<EnemyBase>();
}
