using ProjectBubble.Core.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Core.Collectibles
{
    public class CollectibleHeart : CollectibleDefault
    {
        [SerializeField] private float _healFactor = 1;
        public override bool CanCollect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            if(gameObject.TryGetComponent(out Entity entity))
            {
                if (entity.Health == entity.MaxHealth)
                    return false;
            }
            return base.CanCollect(gameObject, collectibleIndex);
        }

        public override void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            base.Collect(gameObject, collectibleIndex);
            if(gameObject.TryGetComponent(out Entity entity))
            {
                entity.Health += _healFactor;
                //VFX HERE LATER
            }
        }
    }
}
