using ProjectBubble.Core;
using UnityEngine;
using TMPro;

namespace ProjectBubble.Content.UI
{
    public class DeathUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootParent;
        [SerializeField] private TMP_Text _tmpScore;
        [SerializeField] private TMP_Text _tmpWave;
        private void Start()
        {
            GameManager.OnLose += ShowUI;
        }

        private void OnDestroy()
        {
            GameManager.OnLose -= ShowUI;
        }

        private void ShowUI()
        {
            _rootParent.gameObject.SetActive(true);

            //There will be button you press.
            //We'll do all the animations and tween in this function later.
            float score = ScoreManager.GetScore();
            int wave = WaveManager.GetWave();

            string scoreText = $"Score: {score}";
            string waveText = $"Wave: {wave}";

            _tmpScore.text = scoreText;
            _tmpWave.text = waveText;
        }

        //A button will call this function.
        //Don't worry about invoking it from in the code.
        public void Restart()
        {
            GameManager.Restart();
        }
    }
}
