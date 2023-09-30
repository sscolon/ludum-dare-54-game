using System;
using System.Collections;
using UnityEngine;

namespace DDCore
{
    public static class Tween
    {
        private static CoroutineBehaviour _ctx;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            GameObject tweener = new GameObject();
            tweener.name = "tweener";
            GameObject.DontDestroyOnLoad(tweener);
            _ctx = tweener.AddComponent<CoroutineBehaviour>();
        }

        public static void Scale(GameObject gameObject, Vector3 startScale, Vector3 endScale, EaseType easeType, float duration = 1f, Action onComplete = null)
        {
            gameObject.transform.localScale = startScale;
            _ctx.StartCoroutine(ScaleRoutine());
            IEnumerator ScaleRoutine()
            {
                float time = 0f;
                while (time < duration)
                {
                    time += Time.unscaledDeltaTime;
                    float completionTime = time / duration;
                    float easedTime = Easing.Calculate(completionTime, easeType);
                    Vector3 easedScale = Vector3.Lerp(startScale, endScale, easedTime);

                    if (gameObject == null)
                        yield break;
                    gameObject.transform.localScale = easedScale;
                    yield return null;
                }

                onComplete?.Invoke();
            }
        }

        public static void Move(GameObject gameObject, Vector3 startPos, Vector3 endPos, EaseType easeType, Action onComplete = null)
        {
            gameObject.transform.position = startPos;
            _ctx.StartCoroutine(ScaleRoutine());
            IEnumerator ScaleRoutine()
            {
                float time = 0f;
                while (time < 1.0f)
                {
                    time += Time.unscaledDeltaTime;
                    float easedTime = Easing.Calculate(time, easeType);
                    Vector3 easedPos = Vector3.Lerp(startPos, endPos, easedTime);
                    if (gameObject == null)
                        yield break;

                    gameObject.transform.position = easedPos;
                    yield return null;
                }

                onComplete?.Invoke();
            }
        }
    }
}
