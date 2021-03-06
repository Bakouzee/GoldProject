using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using AudioController;

namespace GoldProject.Rooms
{
    public class Curtain : MonoBehaviour, IInteractable
    {
        static List<Curtain> curtains = new List<Curtain>();
        [SerializeField] private Animator animator;
        [SerializeField] private Light2D light2D;

        private bool opened;
        public bool IsOpened => opened || broken;
        public System.Action onStateChanged;

        private void Awake() => curtains.Add(this);
        private void OnDestroy() => curtains.Remove(this);

        private void Start()
        {
            animator.SetBool("day", true);
            light2D.gameObject.SetActive(false);
        }

        public Transform Transform => transform;
        public bool IsInteractable => true; //opened;
        public bool NeedToBeInRange => true;

        public bool TryInteract()
        {
            if (broken)
                return false;
            
            return SetOpened(!opened);
        }

        #region Open/Close

        public bool SetOpened(bool newOpened)
        {
            if (broken)
                return false;
            
            if (newOpened) Open();
            else Close();

            return true;
        }

        private void Open()
        {
            if (opened)
                return;
            opened = true;

            AudioManager.Instance.PlayWindowSound(WindowAudioTracks.W_Open);

            animator.SetTrigger("open");
            light2D.gameObject.SetActive(true);

            onStateChanged?.Invoke();
        }

        private void Close()
        {
            if (!opened)
                return;
            opened = false;

            AudioManager.Instance.PlayWindowSound(WindowAudioTracks.W_Close);

            animator.SetTrigger("close");
            light2D.gameObject.SetActive(false);

            onStateChanged?.Invoke();
        }

        #endregion

        public bool IsInsideLight(Vector2 worldPosition)
        {
            float zLightAngle = (light2D.transform.eulerAngles.z + 90f);

            for (int i = -1; i < 2; i += 2)
            {
                float radAngle = (zLightAngle + i * light2D.pointLightOuterAngle * 0.5f) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)).normalized;

                Debug.DrawRay(light2D.transform.position, dir, Color.magenta);

                bool inside = Vector2.Dot((worldPosition-(Vector2)light2D.transform.position).normalized, Vector2.Perpendicular(dir) * -i) > 0;
                if (!inside)
                    return false;
            }

            return Vector2.Distance(light2D.transform.position, worldPosition) < light2D.pointLightOuterRadius;
        }

        private bool broken;
        public bool IsBroken => broken;
        public void Break()
        {
            if (broken)
                return;
            broken = true;
            
            animator.SetTrigger("break");
        }
    }
}