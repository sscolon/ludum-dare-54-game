using ProjectBubble.Core.Collectibles;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectBubble.Content.UI
{
    public class CoinCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpCount;
        [SerializeField] private GameObject _target;
        private void Start()
        {
            if (_target.TryGetComponent(out ICollector collector))
            {
                collector.OnCollect += UpdateUI;
            }
        }

        private void OnDestroy()
        {
            if (_target.TryGetComponent(out ICollector collector))
            {
                collector.OnCollect -= UpdateUI;
            }
        }

        private void UpdateUI(Dictionary<int, int> collectibleIndex)
        {
            if (!collectibleIndex.ContainsKey(CollectibleID.Coins))
                return;
            int coinCount = collectibleIndex[CollectibleID.Coins];
            _tmpCount.text = $"{coinCount}";
        }
    }
}
