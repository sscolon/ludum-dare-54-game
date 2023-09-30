using System;
using System.Collections.Generic;

namespace ProjectBubble.Core.Collectibles
{
    internal interface ICollector
    {
        public event Action<Dictionary<int, int>> OnCollect;
    }
}
