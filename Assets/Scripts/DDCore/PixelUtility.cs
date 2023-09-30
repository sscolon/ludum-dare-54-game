using UnityEngine;

namespace DDCore
{
    public static class PixelUtility
    {
        public const float PPU = 16;
        public static Vector3 PixelSnap(Vector3 targetPosition)
        {
            Vector3 pixelPosition = targetPosition;
            pixelPosition.x = (Mathf.Round(pixelPosition.x * PPU) / PPU);
            pixelPosition.y = (Mathf.Round(pixelPosition.y * PPU) / PPU);
            pixelPosition.z = (Mathf.Round(pixelPosition.z * PPU) / PPU);
            return pixelPosition;
        }

        public static Vector3 PixelSnap(Transform parent, SpriteRenderer spriteRenderer)
        {
            float x = (Mathf.Round(parent.position.x * PPU) / PPU) - parent.position.x;
            float y = (Mathf.Round(parent.position.y * PPU) / PPU) - parent.position.y;
            Vector3 finalPosition = new Vector3(x * spriteRenderer.transform.lossyScale.x, y * spriteRenderer.transform.lossyScale.y, 0);
            return finalPosition;
        }
    }
}
