using System;
namespace ProjectBubble.Core
{
    public static class ScoreManager
    {
        private static float _score;
        public static event Action<float> OnScoreValueChanged;
        public static void ResetScore()
        {
            _score = 0;
            OnScoreValueChanged?.Invoke(_score);
        }

        public static float GetScore()
        {
            return _score;
        }

        public static void ModifyScore(float score)
        {
            _score += score;
            OnScoreValueChanged?.Invoke(_score);
        }
    }
}
