using UnityEngine;
using UnityEngine.UI;

namespace ProjectBubble.MainPlayer.UI
{
    public class BubbleUI : MonoBehaviour
    {
        [SerializeField] private Image _bubbleOuter;
        [SerializeField] private Image _bubbleInner;
        public Sprite Outer
        {
            get => _bubbleOuter.sprite;
            set => _bubbleOuter.sprite = value;
        }

        public Sprite Inner
        {
            get => _bubbleInner.sprite;
            set => _bubbleInner.sprite = value;
        }
    }
}
