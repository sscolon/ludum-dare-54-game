using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DDCore
{
    public class SceneLoader : MonoBehaviour
    {
        private Transition _transition;

        /// <summary>
        /// Called after the transition
        /// </summary>
        public event Action SceneLoadStarted;

        /// <summary>
        /// Called after the scene finishes loading.
        /// </summary>
        public event Action SceneLoadFinished;

        public static SceneLoader Main { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            using (var watch = new Watch("Loading SceneLoader"))
            {
                var gameObject = new GameObject();
                Main = gameObject.AddComponent<SceneLoader>();
                Main._transition = Transition.Main;
                gameObject.hideFlags = HideFlags.HideInHierarchy;
                DontDestroyOnLoad(Main);
            }
        }

        public void LoadScene(string scene, List<string> additive = null, bool useTransition = true, Action onFinish = null)
        {
            StartCoroutine(LoadSceneRoutine(scene, additive, useTransition, onFinish));
        }

        public IEnumerator LoadSceneRoutine(string scene,
            List<string> additive = null, bool useTransition = true, Action onFinish = null)
        {
            if (useTransition)
            {
                yield return StartCoroutine(_transition.TransitionIn());
            }

            SceneLoadStarted?.Invoke();
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            while (!operation.isDone)
            {
                yield return null;
            }


            //Load Additive Scenes
            if (additive != null)
            {
                for (int i = 0; i < additive.Count; i++)
                {
                    AsyncOperation additiveOperation = SceneManager.LoadSceneAsync(additive[i], LoadSceneMode.Additive);
                    while (!additiveOperation.isDone)
                    {
                        yield return null;
                    }
                }
            }

            if (onFinish != null)
            {
                onFinish();
                yield return new WaitForEndOfFrame();
            }

            SceneLoadFinished?.Invoke();
            if (useTransition)
            {
                yield return StartCoroutine(_transition.TransitionOut());
            }
        }
    }
}