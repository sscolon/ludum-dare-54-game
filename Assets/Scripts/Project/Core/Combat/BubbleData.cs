using UnityEngine;

namespace ProjectBubble.Core.Combat
{
    [CreateAssetMenu(menuName = "ProjectBubble/Bubble Type")]
    public class BubbleData : ScriptableObject
    {
        public string title;
        public Sprite icon;
        public GameObject prefab;
    }
}
