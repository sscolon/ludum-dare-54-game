using ProjectBubble.Core.Combat;
using ProjectBubble.MainPlayer;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Content.Collectibles
{
    public class CollectibleShopBubble : CollectibleShopItem
    {
        [SerializeField] private BubbleData[] _bubblesToSell;
        public override void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            base.Collect(gameObject, collectibleIndex);
            if (gameObject.TryGetComponent(out Player player))
            {
                int randIndex = Random.Range(0, _bubblesToSell.Length);
                BubbleData bubbleToSell = _bubblesToSell[randIndex];
                player.NewBubble(bubbleToSell);
            }
        }
    }
}
