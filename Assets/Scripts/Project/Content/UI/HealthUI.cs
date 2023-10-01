using ProjectBubble.Core.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBubble.Content.UI
{
    public class HealthUI : MonoBehaviour
    {
        private List<Image> _hearts;
        [SerializeField] private GameObject _target;
        [SerializeField] private Image _heartPrefab;
        [SerializeField] private Sprite _heartFilled;
        [SerializeField] private Sprite _heartEmpty;
        [SerializeField] private Material _spriteWhite;
        private void Start()
        {
            _hearts = new List<Image>();
            if (_target.TryGetComponent(out Entity entity))
            {
                entity.OnHealthValueChanged += UpdateUI;
                entity.OnMaxHealthValueChanged += UpdateMaxUI;
                UpdateMaxUI(entity.MaxHealth);
                UpdateUI(entity.Health);
            }
        }

        private void OnDestroy()
        {
            if (_target == null)
                return;
            if (_target.TryGetComponent(out Entity entity))
            {
                entity.OnHealthValueChanged -= UpdateUI;
                entity.OnMaxHealthValueChanged -= UpdateMaxUI;
            }
        }

        private void UpdateUI(float health)
        {
            int current = (int)health;
            for(int i = 0; i < _hearts.Count; i++)
            {
                if(i < current)
                {
                    _hearts[i].sprite = _heartFilled;
                }
                else
                {
                    _hearts[i].sprite = _heartEmpty;
                }
            }

            StartCoroutine(DoSexyJiggle());
        }

        private void UpdateMaxUI(float maxValue)
        {
            int max = (int)maxValue;
            while(_hearts.Count < max)
            {
                //Gonna use a grid layout group.
                Image heart = Instantiate(_heartPrefab, transform, false);
                heart.sprite = _heartEmpty;
                _hearts.Add(heart);
            }

            while(_hearts.Count > max)
            {
                Destroy(_hearts[_hearts.Count - 1]);
                _hearts.RemoveAt(_hearts.Count - 1);
            }

            StartCoroutine(DoSexyJiggle());
        }

        private IEnumerator DoSexyJiggle()
        {
            const float Jiggle_Speed = 2f;
            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * Jiggle_Speed;
                Vector3 startScale = Vector3.one;
                Vector3 midScale = new Vector3(1.5f, 1.5f, 1.5f);
                Vector3 endScale = Vector3.one;

                Vector3 s1 = Vector3.Lerp(startScale, midScale, elapsedTime);
                Vector3 s2 = Vector3.Lerp(midScale, endScale, elapsedTime);
                Vector3 scale = Vector3.Lerp(s1, s2, elapsedTime);
                transform.localScale = scale;
                yield return null;
            }
        }
    }
}
