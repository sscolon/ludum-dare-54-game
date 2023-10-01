using ProjectBubble.Core.Collectibles;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectBubble.Content.UI
{
    public class CoinCounterUI : MonoBehaviour
    {
        private ICollector _collector;
        [SerializeField] private TMP_Text _tmpCount;
        [SerializeField] private TMP_Text _tmpCountShadow;
        [SerializeField] private GameObject _target;
        private void Start()
        {
            _collector = _target.GetComponent<ICollector>();
            _collector.OnCollect += UpdateUI;
        }

        private void OnDestroy()
        {
            _collector.OnCollect -= UpdateUI;
        }

        private void UpdateUI(Dictionary<int, int> collectibleIndex)
        {
            if (!collectibleIndex.ContainsKey(CollectibleID.Coins))
                return;
            int coinCount = collectibleIndex[CollectibleID.Coins];
            _tmpCount.text = $"{coinCount}";
            _tmpCountShadow.text = _tmpCount.text;
        }
    }
}
