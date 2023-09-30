using ProjectBubble.Core;
using TMPro;
using UnityEngine;

namespace ProjectBubble.Content.UI
{
    public class VictoryUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootParent;
        [SerializeField] private TMP_Text _tmpVictory;
        [SerializeField] private TMP_Text _tmpScore;
        [SerializeField] private TMP_Text _tmpWave;
        private void Start()
        {
            GameManager.OnVictory += ShowUI;
        }

        private void OnDestroy()
        {
            GameManager.OnVictory -= ShowUI;
        }

        private void ShowUI()
        {
            _rootParent.gameObject.SetActive(true);

            //TODO:
            //Animate Victory Text
            //Animate Score Text
            //Thanks for playing message and an option to rerun.


            //There will be button you press.
            //We'll do all the animations and tween in this function later.
            float score = ScoreManager.GetScore();
            int wave = WaveManager.GetWave();

            string scoreText = $"Score: {score}";
            string waveText = $"Wave: {wave}";

            _tmpScore.text = scoreText;
            _tmpWave.text = waveText;
        }
    }
}
