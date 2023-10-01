using ProjectBubble.Core;
using TMPro;
using UnityEngine;

namespace ProjectBubble.Content.UI
{
    public class RestTimerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpTimer;
        private void Start()
        {
            WaveManager.OnRestStart += ShowUI;
            WaveManager.OnRestStop += HideUI;
            WaveManager.OnRestCountdown += UpdateUI;
        }
        private void OnDestroy()
        {
            WaveManager.OnRestStart -= ShowUI;
            WaveManager.OnRestStop -= HideUI;
            WaveManager.OnRestCountdown -= UpdateUI;
        }

        private void ShowUI()
        {
            _tmpTimer.gameObject.SetActive(true);
        }

        private void HideUI()
        {
            _tmpTimer.gameObject.SetActive(false);
        }

        private void UpdateUI(float time)
        {
             time = Mathf.Ceil(time);
            _tmpTimer.text = $"{time}s";
        }
    }
}
