using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class BubbleCamera : MonoBehaviour
    {
        private static BubbleCamera _instance;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private RenderTexture _renderTexture;
        private void OnEnable()
        {
            _instance = this;
        }

        public static void MovePosition(Vector3 position)
        {
            _instance.transform.position = position + _instance._offset;
        }

        public static Sprite TakeSnapshot()
        {
            Texture2D texture2D = new Texture2D(_instance._renderTexture.width, _instance._renderTexture.height);
            texture2D.filterMode = FilterMode.Point;

            RenderTexture.active = _instance._renderTexture;
            Rect rect = new Rect(0, 0, _instance._renderTexture.width, _instance._renderTexture.height);
            texture2D.ReadPixels(rect, 0, 0);
            texture2D.Apply();

            Sprite sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f), 16);
            return sprite;
        }
    }
}
