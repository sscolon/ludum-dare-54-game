using DDCore;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBubble.Content.UI
{
    public class MusicSliderUI : MonoBehaviour
    {
        private Slider _slider;
        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(ChangeVolume);
            _slider.value = AudioManager.GetMusicVolume();
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(ChangeVolume);
        }

        private void ChangeVolume(float value)
        {
            AudioManager.SetMusicVolume(value);
        }
    }
}
