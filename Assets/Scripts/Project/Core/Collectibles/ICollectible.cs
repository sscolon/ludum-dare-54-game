using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Core.Collectibles
{
    internal interface ICollectible
    {
        bool CanCollect(GameObject gameObject, Dictionary<int, int> collectibleIndex);
        void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex);
    }
}
