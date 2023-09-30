using UnityEngine;

namespace ProjectBubble.Core.Combat
{
    public class BubbledObject
    {
        public readonly GameObject gameObject;
        public readonly Vector3 offset;
        public BubbledObject(GameObject gameObject, Vector3 offset)
        {
            this.gameObject = gameObject;
            this.offset = offset;
        }
    }
}
