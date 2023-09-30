using ProjectBubble.Core.Collectibles;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Content.Collectibles
{
    public class CollectibleCoin : CollectibleDefault
    {
        [SerializeField] private int _value;
        public override void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            base.Collect(gameObject, collectibleIndex);
            if (collectibleIndex.ContainsKey(CollectibleID.Coins))
                collectibleIndex[CollectibleID.Coins] += _value;
            else
            {
                collectibleIndex.Add(CollectibleID.Coins, _value);
            }

            //TODO VFX HERE LATER
        }
    }
}
