using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace VFX
{
    public class AmbianceLight : MonoBehaviour
    {
        [SerializeField] private Light2D[] lights;
        
        [Header("Day")]
        [SerializeField] private LightData dayLight;
        [SerializeField] private GameObject[] dayObjects;
        
        [Header("Night")]
        [SerializeField] private LightData nightLight;
        [SerializeField] private GameObject[] nightObjects;

        private void Awake() => LightsManager.lights.Add(this);
        private void OnDestroy() => LightsManager.lights.Remove(this);

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            // gameManager.OnDayStart += OnDayStart;
            // gameManager.OnNightStart += OnNightStart;

            foreach (Light2D light2D in lights)
            {
                light2D.color = dayLight.color;
                light2D.intensity = dayLight.intensity;
            }
        }

        private void SetActive(GameObject[] array, bool newActive)
        {
            foreach (GameObject element in array)
                element.SetActive(newActive);
        }

        private void OnDayStart() => ChangeLights(true);
        private void OnNightStart() => ChangeLights(false);
        private void ChangeLights(bool day)
        {
            SetActive(dayObjects, day);
            SetActive(nightObjects, !day);
        }

        public void LerpLights(bool day, float t) => 
            Lerp(day ? dayLight : nightLight, day ? nightLight : dayLight, t);
        private void Lerp(LightData a, LightData b, float t)
        {
            t = Mathf.Clamp01(t);
            Color newColor = Color.Lerp(a.color, b.color, t);
            float newIntensity = Mathf.Lerp(a.intensity, b.intensity, t);

            foreach (var light2D in lights)
            {
                light2D.color = newColor;
                light2D.intensity = newIntensity;
            }
        }
        
        [System.Serializable]
        private class LightData
        {
            public Color color;
            public float intensity;
        }
    }
}