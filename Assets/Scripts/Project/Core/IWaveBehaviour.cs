using System;

namespace ProjectBubble.Core
{
    public interface IWaveBehaviour
    {
        public int MinSpawnWave { get; }
        public int MaxSpawnWave { get; }
        public float SpawnWeight { get; }
        public event Action OnClear;
    }
}
