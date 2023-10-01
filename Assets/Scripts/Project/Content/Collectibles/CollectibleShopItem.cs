
using ProjectBubble.Core.Collectibles;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectBubble.Content.Collectibles
{
    public class CollectibleShopItem : CollectibleDefault
    {
        private int _cost;
        [SerializeField] private int _minCost;
        [SerializeField] private int _maxCost;
        [SerializeField] private TMP_Text _priceTmp;

        protected override void Start()
        {
            base.Start();
            _cost = Random.Range(_minCost, _maxCost);
            _priceTmp.text = $"{_cost}";
        }

        public override bool CanCollect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            if (!collectibleIndex.ContainsKey(CollectibleID.Coins))
                return false;
            if (collectibleIndex[CollectibleID.Coins] < _cost)
                return false;
            return base.CanCollect(gameObject, collectibleIndex);
        }

        public override void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            base.Collect(gameObject, collectibleIndex);
            collectibleIndex[CollectibleID.Coins] -= _cost;
            //VFX here later
        }
    }
}
