using UnityEngine;

namespace ProjectBubble.Core
{
    public static class PixelUtil
    {
        public const float PPU = 16;
        public static float PixelRound(float target)
        {
            float pixelFloat = (Mathf.Round(target * PPU) / PPU);
            return pixelFloat;
        }

        public static Vector3 PixelRound(Vector3 targetPosition)
        {
            Vector3 pixelPosition = targetPosition;
            pixelPosition.x = (Mathf.FloorToInt(pixelPosition.x * PPU) / PPU);
            pixelPosition.y = (Mathf.FloorToInt(pixelPosition.y * PPU) / PPU);
            pixelPosition.z = (Mathf.FloorToInt(pixelPosition.z * PPU) / PPU);
            return pixelPosition;
        }
        public static Vector3 PixelFloor(Vector3 targetPosition)
        {
            Vector3 pixelPosition = targetPosition;
            pixelPosition.x = (Mathf.Floor(pixelPosition.x * PPU) / PPU);
            pixelPosition.y = (Mathf.Floor(pixelPosition.y * PPU) / PPU);
            pixelPosition.z = (Mathf.Floor(pixelPosition.z * PPU) / PPU);
            return pixelPosition;
        }
        public static Vector3 PixelRound(Transform parent, Transform child)
        {
            float x = (Mathf.FloorToInt(parent.position.x * PPU) / PPU) - parent.position.x;
            float y = (Mathf.FloorToInt(parent.position.y * PPU) / PPU) - parent.position.y;
            Vector3 finalPosition = new Vector3(x * child.lossyScale.x, y * child.lossyScale.y, 0);
            return finalPosition;
        }
    }
}
