using ProjectBubble.Core.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.MainPlayer.UI
{
    public class BubbleQueueUI : MonoBehaviour
    {
        private Queue<BubbleUI> _bubbleUIs;
        [SerializeField] private Player _player;
        [SerializeField] private BubbleUI _bubbleUIPrefab;
        private void Start()
        {
            _bubbleUIs = new();
            _player.OnQueue += EnqueueUI;
            _player.OnDequeue += DequeueUI;
        }

        private void OnDestroy()
        {
            _player.OnQueue -= EnqueueUI;
            _player.OnDequeue -= DequeueUI;
        }

        private void EnqueueUI(BubbleData bubbleData)
        {
            BubbleUI ui = Instantiate(_bubbleUIPrefab, transform, false);
            //We can do animation here later.
            _bubbleUIs.Enqueue(ui);
        }

        private void DequeueUI(BubbleData bubbleData)
        {
            BubbleUI ui = _bubbleUIs.Dequeue();
            //We can do animation here later.
            Destroy(ui.gameObject);
        }
    }
}
