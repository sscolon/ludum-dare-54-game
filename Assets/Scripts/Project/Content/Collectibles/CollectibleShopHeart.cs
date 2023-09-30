using ProjectBubble.Core.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Content.Collectibles
{
    public class CollectibleShopHeart : CollectibleShopItem
    {
        [SerializeField] private float _healFactor;
        public override bool CanCollect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            if (gameObject.TryGetComponent(out Entity entity))
            {
                if (entity.Health == entity.MaxHealth)
                    return false;
            }
            return base.CanCollect(gameObject, collectibleIndex);
        }

        public override void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            base.Collect(gameObject, collectibleIndex);
            if (gameObject.TryGetComponent(out Entity entity))
            {
                entity.Health += _healFactor;
                //VFX HERE LATER
            }
        }
    }
}
