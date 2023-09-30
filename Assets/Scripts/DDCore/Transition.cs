using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DDCore
{
    public class Transition : MonoBehaviour
    {
        private static Sprite _screenOverlaySprite;
        private static Transition _transition;
        private Image _blackScreen;
        private const float TRANSITION_SPEED = 4.5f;
        public static Transition Main
        {
            get
            {
                if (_transition == null)
                {
                    using (var watch = new Watch("Loading Scene Transition"))
                    {
                        Texture2D texture = new Texture2D(16, 16);
                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                texture.SetPixel(x, y, Color.white);
                            }
                        }

                        texture.Apply();
                        _screenOverlaySprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16f);

                        var gameObject = new GameObject("ui_black_screen_transition");
                        gameObject.name = "manager_transition";
                        _transition = gameObject.AddComponent<Transition>();
                        gameObject.hideFlags = HideFlags.HideInHierarchy;
                        DontDestroyOnLoad(_transition);
                    }
                }

                return _transition;
            }
        }

        private Image GetBlackScreen()
        {
            GameObject screen = new GameObject();
            GameObject.DontDestroyOnLoad(screen);
            Canvas canvas = screen.AddComponent<Canvas>();
            canvas.sortingOrder = 9999;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject imageObject = new GameObject();
            imageObject.transform.SetParent(screen.transform);

            Image b = imageObject.AddComponent<Image>();
            b.transform.localScale = new Vector3(9999, 9999, 1);
            b.sprite = _screenOverlaySprite;
            b.color = Color.clear;
            return b;
        }

        private IEnumerator TransitionRoutine(IEnumerator routine)
        {
            yield return StartCoroutine(TransitionIn());
            yield return StartCoroutine(routine);
            yield return StartCoroutine(TransitionOut());
        }

        public void DoTransition(IEnumerator routine)
        {
            StartCoroutine(TransitionRoutine(routine));
        }

        public void DoTransition(Action callback)
        {
            StartCoroutine(Routine());
            IEnumerator Routine()
            {
                yield return StartCoroutine(TransitionIn());
                callback();
                yield return StartCoroutine(TransitionOut());
            }
        }

        public IEnumerator TransitionIn(float speed = TRANSITION_SPEED)
        {
            if (_blackScreen == null)
            {
                _blackScreen = GetBlackScreen();
            }
            else
            {
                yield break;
            }

            float a = 0.0f;
            _blackScreen.color = _blackScreen.color.ChangeAlpha(a);
            while (a < 1.0f)
            {
                a += Time.unscaledDeltaTime * speed;
                _blackScreen.color = _blackScreen.color.ChangeAlpha(a);
                yield return null;
            }
        }

        public IEnumerator TransitionOut(float speed = TRANSITION_SPEED)
        {
            if (_blackScreen == null)
            {
                yield break;
            }

            float a = 1.0f;
            while (a > 0f)
            {
                a -= Time.unscaledDeltaTime * speed;
                if (_blackScreen == null)
                {
                    yield break;
                }

                _blackScreen.color = _blackScreen.color.ChangeAlpha(a);
                yield return null;
            }

            //Destroy the object so we aren't losing frames for no reason.
            Destroy(_blackScreen.transform.root.gameObject);
        }
    }
}
