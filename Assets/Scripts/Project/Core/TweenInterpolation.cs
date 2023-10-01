using DDCore;
using System;
using UnityEngine;

namespace ProjectBubble.Core
{
    [Serializable]
    public class TweenInterpolation
    {
        public EaseType easeType;
        public InterpolationType interpolationType;
        public float duration;
        public float startFloat;
        public float endFloat;
        public Vector3 startVector;
        public Vector3 endVector;
        public void Apply(float elapsedTime, Transform transform)
        {
            float time = Mathf.PingPong(elapsedTime / duration, 1f);
            float easedTime = Easing.Calculate(time, easeType);
            switch (interpolationType)
            {
                case InterpolationType.LocalPosition:
                    Vector3 localPosition = Vector3.Lerp(startVector, endVector, easedTime);
                    transform.localPosition = localPosition;
                    break;
                case InterpolationType.LocalScale:
                    Vector3 localScale = Vector3.Lerp(startVector, endVector, easedTime);
                    transform.localScale = localScale;
                    break;
                case InterpolationType.LocalRotation:
                    float z = Mathf.Lerp(startFloat, endFloat, easedTime);
                    Quaternion rot = Quaternion.Euler(0, 0, z);
                    transform.localRotation = rot;
                    break;
            }
        }

        public void Undo(Transform transform)
        {
            switch (interpolationType)
            {
                case InterpolationType.LocalPosition:
                    transform.localPosition = Vector3.zero;
                    break;
                case InterpolationType.LocalScale:
                    transform.localScale = Vector3.one;
                    break;
                case InterpolationType.LocalRotation:
                    transform.localRotation = Quaternion.identity;
                    break;
            }
        }

        public enum InterpolationType
        {
            LocalPosition = 0,
            LocalScale = 1,
            LocalRotation = 2
        }
    }
}
