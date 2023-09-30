using System;
using UnityEngine;

namespace DDCore
{
    [Serializable]
    public class Noise
    {
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public float Scale { get; set; } = 1f;
        public int Width { get; set; }
        public int Height { get; set; }
        public float Sample(float x, float y)
        {
            float xCoord = (XOffset + x) / Width * Scale;
            float yCoord = (YOffset + y) / Height * Scale;
            float sample = Mathf.PerlinNoise(xCoord, yCoord);
            return sample;
        }
    }
}
