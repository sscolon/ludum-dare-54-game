using ProjectBubble.Core;
using TMPro;
using UnityEngine;

namespace ProjectBubble.Content.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpScore;
        private void Start()
        {
            ScoreManager.OnScoreValueChanged += UpdateUI;
            UpdateUI(ScoreManager.GetScore());
        }

        private void OnDestroy()
        {
            ScoreManager.OnScoreValueChanged -= UpdateUI;
        }

        private void UpdateUI(float score)
        {
            _tmpScore.text = $"Score - {score}";
        }
    }
}
