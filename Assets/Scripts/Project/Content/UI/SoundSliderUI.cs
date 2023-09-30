using DDCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBubble.Content.UI
{
    public class SoundSliderUI : MonoBehaviour
    {
        private Slider _slider;
        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(ChangeVolume);
            _slider.value = AudioManager.GetSoundVolume();
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(ChangeVolume);
        }

        private void ChangeVolume(float value)
        {
            AudioManager.SetSoundVolume(value);
        }
    }
}
