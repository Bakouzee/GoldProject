using System;
using System.Collections.Generic;
using UnityEngine;
using GoldProject;

namespace VFX
{
    public class LightsManager : SingletonBase<LightsManager>
    {
        public static List<AmbianceLight> lights = new List<AmbianceLight>();
        
        [SerializeField] private int lerpTurnDuration;
        private int halfLerpTurnDuration;
        
        private bool lerping;
        private bool beganLerpOnDay;
        private float currentT;
        
        private void Start()
        {
            halfLerpTurnDuration = Mathf.FloorToInt(lerpTurnDuration * 0.5f);
            
            GameManager.Instance.OnLaunchedTurn += OnLaunchedTurn;
        }

        void OnLaunchedTurn(int turnCount, int turnsPerPhase)
        {
            int diff = turnsPerPhase - turnCount;
            if (!lerping)
            {
                if (diff <= halfLerpTurnDuration)
                {
                    beganLerpOnDay = GameManager.dayState == GameManager.DayState.DAY;
                    lerping = true;
                    currentT = 0;
                    // Debug.Log("Started lights lerp");
                }
            }


            if (!lerping)
                return;

            currentT += 1 / (float) lerpTurnDuration;
            // Debug.Log($"t: {currentT} --- diff: {diff} --- {turnCount}/{turnsPerPhase}");

            foreach (var ambianceLight in lights)
            {
                ambianceLight.LerpLights(beganLerpOnDay, currentT);
            }

            if (currentT >= 1f) lerping = false;
        }
        
        
    }
}