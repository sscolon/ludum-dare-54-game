using ProjectBubble.Core.Combat;
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
        }
    }
}
