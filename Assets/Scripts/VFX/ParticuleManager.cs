using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticuleManager : SingletonBase<ParticuleManager>
{
    public enum ParticuleType
    {
        P_Death,
        E_Scared,
        E_Terrified,
        E_Death
    }

    [System.Serializable]
    public struct Particule
    {
        // For GDs
        public ParticuleType Type;
        public ParticleSystem Particle;
    }
    [SerializeField] public Particule[] particules;


    public void OnPlayerDeathParticule()
    {
        for(int i = 0; i < particules.Length; i++)
        {
            if(particules[i].Type == ParticuleType.P_Death)
            {
                particules[i].Particle.Play();
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
                particules[i].Particle.Play();
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
                particules[i].Particle.Play();
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
                particules[i].Particle.Play();
                return;
            }

        }
    }
}
