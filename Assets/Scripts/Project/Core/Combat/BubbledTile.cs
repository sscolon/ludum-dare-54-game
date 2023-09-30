using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectBubble.Core.Combat
{
    public class BubbledTile
    {
        public readonly TileBase tile;
        public readonly Vector3Int offset;
        public readonly Vector3Int originalPosition;
        public BubbledTile(TileBase tile, Vector3Int originalPosition, Vector3Int offset)
        {
            this.tile = tile;
            this.originalPosition = originalPosition;
            this.offset = offset;
        }
    }
}
