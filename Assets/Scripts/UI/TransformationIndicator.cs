using System;
using UnityEngine;

namespace GoldProject.UI
{
    public class TransformationIndicator : MonoBehaviour
    {
        [Header("Animations")]
        [SerializeField] private bool hasAnimator;
        [SerializeField] private Animator animator;
        [SerializeField] private string animationTriggerName;
        [SerializeField] private string animationResetName;

        private void Start()
        {
            if (!hasAnimator)
            {
                Debug.LogError("No animator", this);
                return;
            }
            
            if (!animator) animator = GetComponentInChildren<Animator>();
        }

        public void SetIndicator(bool canTransform)
        {
            if (hasAnimator)
            {
                if (!animator)
                    return;
                
                animator.SetTrigger(canTransform ? animationTriggerName : animationResetName);
            }
        }
    }
}