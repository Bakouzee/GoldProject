using UnityEngine;
using UnityEngine.Rendering;

namespace GoldProject
{
    [System.Serializable]
    public class Cooldown
    {
        public float cooldownDuration;
        [Attributes.ReadOnly] public float time;

        public bool HasCooldown() => time <= Time.time;
        public void SetCooldown() => time = Time.time + cooldownDuration;
    }
}