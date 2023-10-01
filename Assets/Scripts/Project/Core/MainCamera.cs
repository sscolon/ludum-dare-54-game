using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class MainCamera : MonoBehaviour
    {
        private static MainCamera _instance;
        private Vector3 _shakeOffset;
        private List<Shake> _shakesToRemove = new List<Shake>();
        private List<Shake> _shakes = new List<Shake>();

        [field: SerializeField]
        public Transform Target { get; set; }

        [field: SerializeField]
        public Vector3 TargetOffset { get; set; } = new Vector3(0, 0, -10);

        private void OnEnable()
        {
            _instance = this;
            _shakes = new();
            _shakesToRemove = new();
        }

        private void LateUpdate()
        {
            CalculateCameraShake();
            transform.position = Target.position + TargetOffset + _shakeOffset;
            transform.position = PixelUtil.PixelRound(transform.position);
        }

        private void CalculateCameraShake()
        {
            _shakeOffset = Vector3.zero;
            for (int i = 0; i < _shakes.Count; i++)
            {
                Shake shake = _shakes[i];
                float xOffset = UnityEngine.Random.Range(-shake.strength, shake.strength);
                float yOffset = UnityEngine.Random.Range(-shake.strength, shake.strength);
                _shakeOffset += new Vector3(xOffset, yOffset);
                shake.time -= Time.deltaTime;
                if (shake.time <= 0f)
                {
                    _shakesToRemove.Add(shake);
                }
            }

            for (int i = 0; i < _shakesToRemove.Count; i++)
            {
                Shake shake = _shakesToRemove[i];
                _shakes.Remove(shake);
            }

            _shakesToRemove.Clear();
        }

        /// <summary>
        /// Shakes the camera.
        /// </summary>
        /// <param name="pixelStrength"></param>
        /// <param name="duration"></param>
        public static void Screenshake(int pixelStrength, float duration)
        {
            float strength = pixelStrength / 16f;
            Shake shake = new Shake(strength, duration);
            _instance._shakes.Add(shake);
        }

        private class Shake
        {
            public float strength;
            public float time;
            public Shake(float strength, float time)
            {
                this.strength = strength;
                this.time = time;
            }
        }

    }
}
