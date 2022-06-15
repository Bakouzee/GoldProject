using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticuleManager : SingletonBase<ParticuleManager>
{
    public enum ParticuleType
    {
        P_Death,
        P_Heal,
        E_Scared,
        E_Terrified,
        E_Death
    }

    [System.Serializable]
    public struct Particule
    {
        // For GDs
        public ParticuleType Type;
        public Animator Particle;
        public string animTrigger;
    }
    [SerializeField] public Particule[] particules;

    private void Start()
    {
        for(int i = 0; i < particules.Length; i++)
        {
            particules[i].Particle.gameObject.SetActive(false);
        }
    }

    public void OnPlayerDeath()
    {
        for(int i = 0; i < particules.Length; i++)
        {
            if(particules[i].Type == ParticuleType.P_Death)
            {
                particules[i].Particle.gameObject.SetActive(true);
                particules[i].Particle.SetTrigger(particules[i].animTrigger);
                return;
            }

        }
    }

    public void OnPlayerHeal()
    {
        Debug.Log("Particule!");
        for (int i = 0; i < particules.Length; i++)
        {
            if (particules[i].Type == ParticuleType.P_Heal)
            {
                particules[i].Particle.gameObject.SetActive(true);

                particules[i].Particle.SetTrigger(particules[i].animTrigger);
                return;
            }

        }
    }

    public void OnEnemyScared()
    {
        for (int i = 0; i < particules.Length; i++)
        {
            if (particules[i].Type == ParticuleType.E_Scared)
            {
                particules[i].Particle.gameObject.SetActive(true);

                particules[i].Particle.SetTrigger(particules[i].animTrigger);
                return;
            }

        }
    }

    public void OnEnemyTerrified()
    {
        for (int i = 0; i < particules.Length; i++)
        {
            if (particules[i].Type == ParticuleType.E_Terrified)
            {
                particules[i].Particle.gameObject.SetActive(true);

                particules[i].Particle.SetTrigger(particules[i].animTrigger);
                return;
            }

        }
    }

    public void OnEnemyDeath()
    {
        for (int i = 0; i < particules.Length; i++)
        {
            if (particules[i].Type == ParticuleType.E_Death)
            {
                particules[i].Particle.gameObject.SetActive(true);

                particules[i].Particle.SetTrigger(particules[i].animTrigger);
                return;
            }

        }
    }
}
