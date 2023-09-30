using System.Collections.Generic;
using UnityEngine;

namespace DDCore
{
    public class MainCamera : MonoBehaviour
    {
        public const float PIXELS_PER_UNIT = 16;
        private bool _isLocked;
        private bool _doSmooth;
        private int _cameraShake;
        private Vector3 _vel;
        private Vector3 _targetOffset;
        private Vector3 _shakeOffset;
        private Vector3 _realPosition;
        private Vector3 _lockPosition;
        private Rigidbody2D _targetBody;
        private List<Shake> _shakesToRemove = new List<Shake>();
        private List<Shake> _shakes = new List<Shake>();

        [SerializeField] private int _cameraStyle;
        [SerializeField] private float _smoothTime = 5f;
        [SerializeField] private float _mouseLookDistance;
        [SerializeField] private Vector3 _targetLookOffset;
        [SerializeField] private Transform _target;

        public static MainCamera Instance { get; private set; }
        private void OnEnable()
        {
            Instance = this;
            _realPosition = transform.position;
        }

        private void LateUpdate()
        {
            Follow();
        }

        private void FollowLock()
        {
            if (_doSmooth)
            {
                FollowSmooth();
                return;
            }

            CalculateCameraShake();
            Vector3 desiredPosition = _lockPosition + _shakeOffset;
            transform.position = PixelUtility.PixelSnap(desiredPosition);
        }

        private void FollowSmooth()
        {
            Vector3 desiredPosition = _isLocked ? _lockPosition : _target.position + _targetOffset + _shakeOffset;
            if (_targetBody != null)
            {
                Vector3 velocity = _targetBody.velocity;
                Vector3 velocityOffset = velocity / PIXELS_PER_UNIT;
                desiredPosition += velocityOffset;
            }
            Vector3 smoothedPosition = Vector3.SmoothDamp(_realPosition, PixelUtility.PixelSnap(desiredPosition), ref _vel, _smoothTime * Time.deltaTime);
            _realPosition = PixelUtility.PixelSnap(smoothedPosition);
            transform.position = PixelUtility.PixelSnap(_realPosition);

            float distance = Vector3.Distance(_realPosition, desiredPosition);
            if (distance <= 1 / 16f)
            {
                _doSmooth = false;
            }
        }

        private void Follow()
        {
            if (_isLocked)
            {
                FollowLock();
                return;
            }

            if (_doSmooth)
            {
                FollowSmooth();
                return;
            }

            if (_target == null)
            {
                SetDefaultTarget();
                return;
            }

            //Copy target offset to a new value so we can do things with in
            _targetOffset = _targetLookOffset;
            CalculateCameraShake();

            switch (_cameraStyle)
            {
                default:
                case 0:
                    Vector3 desiredPosition = _target.position + _targetOffset + _shakeOffset;
                    if (_targetBody != null)
                    {
                        Vector3 velocity = _targetBody.velocity;
                        Vector3 velocityOffset = velocity / PIXELS_PER_UNIT;
                        desiredPosition += velocityOffset;
                    }
                    Vector3 smoothedPosition = Vector3.SmoothDamp(_realPosition, PixelUtility.PixelSnap(desiredPosition), ref _vel, _smoothTime * Time.deltaTime);
                    _realPosition = PixelUtility.PixelSnap(smoothedPosition);
                    transform.position = PixelUtility.PixelSnap(_realPosition);
                    break;
                case 1:
                    Vector3 snapPosition = _target.position + _targetOffset + _shakeOffset;
                    transform.position = PixelUtility.PixelSnap(snapPosition);
                    break;
            }
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

        private void SetDefaultTarget()
        {
            
        }

        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }

        public void ForcePosition(Vector3 position)
        {
            transform.position = position + _targetLookOffset;
            _realPosition = transform.position;
        }

        public void ToggleLockPosition()
        {
            _lockPosition = transform.position;
            _isLocked = !_isLocked;
        }

        public void SmoothInToTarget()
        {
            _doSmooth = true;
        }

        /// <summary>
        /// Shakes the camera.
        /// </summary>
        /// <param name="pixelStrength"></param>
        /// <param name="duration"></param>
        public void Screenshake(int pixelStrength, float duration)
        {
            if (_cameraShake == 0)
                return;

            float strength = pixelStrength / 16f;
            Shake shake = new Shake(strength, duration);
            _shakes.Add(shake);
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

        public static MainCamera GetInstance()
        {
            return Instance;
        }
    }
}