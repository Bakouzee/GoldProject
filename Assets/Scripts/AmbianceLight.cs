using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GoldProject
{
    public class AmbianceLight : MonoBehaviour
    {
        [SerializeField] private Light2D[] dayLights;
        [SerializeField] private Light2D[] nightLights;

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            gameManager.OnDayStart += OnDayStart;
            gameManager.OnNightStart += OnNightStart;
        }

        private void SetActive<T>(T[] array, bool newActive) where T : MonoBehaviour
        {
            foreach (var element in array)
                element.gameObject.SetActive(newActive);
        }

        private void OnDayStart() => ChangeLights(true);
        private void OnNightStart() => ChangeLights(false);
        private void ChangeLights(bool day)
        {
            SetActive(dayLights, day);
            SetActive(nightLights, !day);
        }
    }
}