using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SplashArt
{
    [RequireComponent(typeof(SpriteRenderer))]
    [System.Serializable]
    public class SplashArtManager : SingletonBase<SplashArtManager>
    {
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        [SerializeField] private string animTrigger;

        private Animator anim;
        private Image img;

        private void Start()
        {
            img = GetComponent<Image>();

            anim = GetComponent<Animator>();
        }

        public void SplashArtToChoose(SplashArtType splashArtType)
        {
            switch (splashArtType)
            {
                case SplashArtType.Transformation:
                    img.sprite = sprites[0];
                    break;
                case SplashArtType.Untransformation:
                    img.sprite = sprites[1];
                    break;
                case SplashArtType.Transformation_First_Kill:
                    img.sprite = sprites[2];
                    break;
                case SplashArtType.Death:
                    img.sprite = sprites[3];
                    break;
                default:
                    img.sprite = sprites[0];
                    break;
            }
            Time.timeScale = 0;
            StartCoroutine(PlayAnim());
        }

        private IEnumerator PlayAnim()
        {
            anim.SetTrigger(animTrigger);
            yield return new WaitForSeconds(2f);
            Time.timeScale = 1;
        }
    }
}