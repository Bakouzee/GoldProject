using System;
using AudioController;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Vector2 mixerVolumeMinMax;
    public VolumeSlider[] volumeSliders;

    [Header("Sprites"), SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private void Start()
    {
        foreach (var volumeSlider in volumeSliders)
        {
            volumeSlider?.Init(mixerVolumeMinMax, audioMixer, onSprite, offSprite);
        }
        
        gameObject.SetActive(false);
    }

    [System.Serializable]
    public class VolumeSlider
    {
        public string name;
        public UnityEngine.UI.Slider slider;
        public UnityEngine.UI.Image muteImage;
        [Range(0f, 1f)] public float defaultValue;

        private bool muted;
        
        private float value;
        public float Value
        {
            get => value;
            set
            {
                this.value = Mathf.Clamp01(value);
                if(slider) slider.value = this.value;
            }
        }

        public void Init(Vector2 mixerVolumeMinMax, AudioMixer audioMixer, Sprite onSprite, Sprite offSprite)
        {
            if (slider)
            {
                slider.minValue = 0f;
                slider.maxValue = 1f;
            }
            Value = PlayerPrefs.GetFloat(name, defaultValue);
            SetVolume(Value, mixerVolumeMinMax, audioMixer);
            
            // Set slider value
            if (slider)
            {
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener((newSliderValue)
                    => SetVolume(newSliderValue, mixerVolumeMinMax, audioMixer));
            }
            
            // Set mute image sprite
            muted = PlayerPrefs.GetInt(name + "Muted", 0) == 1;
            if (!muteImage)
                return;
            
            muteImage.sprite = muted ? offSprite : onSprite;
            if (muteImage.TryGetComponent(out UnityEngine.UI.Button button))
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    ToggleMute(mixerVolumeMinMax, audioMixer);
                });
            }
        }

        public void SetVolume(float newVolume, Vector2 mixerVolumeMinMax, AudioMixer audioMixer)
        {
            // Debug.Log("Set Volume");
            Value = newVolume;
            Debug.Log($"{Value}");
            PlayerPrefs.SetFloat(name, Value);
            if (Value <= 0.01f)
                audioMixer.SetFloat(name, -80f);
            else
            {
                var lerp = Mathf.Lerp(mixerVolumeMinMax.x, mixerVolumeMinMax.y, Value);
                audioMixer.SetFloat(name, lerp);
                Debug.Log($"{name}: {lerp}");
            }
        }

        public void ToggleMute(Vector2 mixerVolumeMinMax, AudioMixer audioMixer) 
            => SetMuted(!muted, mixerVolumeMinMax, audioMixer);

        private void SetMuted(bool muted, Vector2 mixerVolumeMinMax, AudioMixer audioMixer)
        {
            this.muted = muted;
            SetVolume(muted ? 0f : 1f, mixerVolumeMinMax, audioMixer);
        }
    }
}
