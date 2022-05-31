using System.Collections.Generic;
using Enemies;

namespace GoldProject
{
    [System.Serializable]
    public struct TypeAndPrefabs<T>
    {
        public TypeAndPrefab<T>[] typeAndPrefabs;
        public Dictionary<T, Enemies.EnemyBase> dict;

        public void Init()
        {
            dict = new Dictionary<T, EnemyBase>();
            foreach (var typeAndPrefab in typeAndPrefabs)
            {
                dict.Add(typeAndPrefab.type, typeAndPrefab.prefab);
            }
        }
    }

    
    [System.Serializable]
    public struct TypeAndPrefab<T>
    {
        public T type;
        public Enemies.EnemyBase prefab;
    }
}