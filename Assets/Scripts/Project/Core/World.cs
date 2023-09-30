using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectBubble.Core
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Tilemap _ground;
        [SerializeField] private Tilemap _undercliff;
        [SerializeField] private TileBase _undercliffTile;
        public static Tilemap Ground { get; private set; }
        public static Tilemap Undercliff { get; private set; }
        public static TileBase UndercliffTile { get; private set; }
        private void OnEnable()
        {
            Ground = _ground;
            Undercliff = _undercliff;
            UndercliffTile = _undercliffTile;
        }
    }
}
