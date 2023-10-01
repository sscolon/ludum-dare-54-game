using UnityEngine;

namespace ProjectBubble.Core
{
    public class TweenInterpolator : MonoBehaviour
    {
        private float _elapsedTime;
        [SerializeField] private bool _alwaysPlay = true;
        [SerializeField] private TweenInterpolation[] _interpolations;
        private void Update()
        {
            if (_alwaysPlay)
            {
                UpdateTween();

            }
        }

        public void UpdateTween()
        {
            _elapsedTime += Time.deltaTime;
            float pingPong = Mathf.PingPong(_elapsedTime, 1f);
            for (int i = 0; i < _interpolations.Length; i++)
            {
                _interpolations[i]?.Apply(pingPong, transform);
            }
        }

    }
}
