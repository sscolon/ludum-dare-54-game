using ProjectBubble.Core.Combat;
using ProjectBubble.MainPlayer;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Content.Collectibles
{
    public class CollectibleShopBubble : CollectibleShopItem
    {
        private BubbleData _bubbleToSell;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BubbleData[] _bubblesToSell;
        protected override void Start()
        {
            base.Start();
            int randIndex = Random.Range(0, _bubblesToSell.Length);
            _bubbleToSell = _bubblesToSell[randIndex];
            _spriteRenderer.sprite = _bubbleToSell.icon;
        }

        public override void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            base.Collect(gameObject, collectibleIndex);
            if (gameObject.TryGetComponent(out Player player))
            {
                player.NewBubble(_bubbleToSell);
            }
        }
    }
}
