using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Core.Combat
{
    [CreateAssetMenu(menuName = "ProjectBubble/Bubble Type")]
    public class BubbleData : ScriptableObject
    {
        public string title;
        public Sprite wrapper;
        public GameObject prefab;
        public List<BubbledObject> BubbledObjects { get; private set; } = new List<BubbledObject>();
        public List<BubbledTile> BubbledTiles { get; private set; } = new List<BubbledTile>();
        public bool HasData()
        {
            return BubbledObjects.Count > 0 || BubbledTiles.Count > 0;
        }

        public void ClearData()
        {
            BubbledObjects.Clear();
            BubbledTiles.Clear();
        }
    }
}
