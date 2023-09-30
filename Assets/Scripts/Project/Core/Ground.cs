using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectBubble.Core
{
    [RequireComponent(typeof(Tilemap))]
    public class Ground : MonoBehaviour
    {
        public static Tilemap Map { get; private set; }
        private void OnEnable()
        {
            Map = GetComponent<Tilemap>();
        }
    }
}
