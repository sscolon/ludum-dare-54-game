using UnityEngine;

namespace ProjectBubble.Core
{
    [CreateAssetMenu(menuName = "ProjectBubble/Loot")]
    public class Loot : ScriptableObject
    {
        [SerializeField] private int _minCount = 1;
        [SerializeField] private int _maxCount = 5;
        [SerializeField] private Entry[] _prefabs;
        private Entry NextEntry()
        {
            float totalWeight = 0;
            for (int i = 0; i < _prefabs.Length; i++)
            {
                var entry = _prefabs[i];
                totalWeight += entry.weight * 1000;
            }

            float randomWeight = Random.Range(0, totalWeight);
            float currentWeight = 0;
            for (int i = 0; i < _prefabs.Length; i++)
            {
                var entry = _prefabs[i];
                currentWeight += entry.weight * 1000;
                if (randomWeight <= currentWeight)
                {
                    return entry;
                }
            }

            return null;
        }

        public void Instantiate(Vector3 position)
        {
            const float Max_Speed = 10;
            int count = Random.Range(_minCount, _maxCount);
            for (int i = 0; i < count; i++)
            {
                Entry entry = NextEntry();
                if (entry == null)
                    continue;
                if (entry.prefab == null)
                    continue;

                GameObject instance = GameObject.Instantiate(entry.prefab, position, entry.prefab.transform.rotation);
                float randX = Random.Range(-1f, 1f);
                float randY = Random.Range(-1f, 1f);
                float speed = Random.Range(-Max_Speed, Max_Speed);
                Vector3 randOffset = new Vector3(randX, randY);
                Vector3 offsetPosition = position + randOffset;
                Vector3 direction = (offsetPosition - position).normalized;
                Vector3 force = direction * speed;
                if (instance.TryGetComponent(out Rigidbody2D body))
                {
                    body.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }

        [System.Serializable]
        private class Entry
        {
            public GameObject prefab;

            [Range(0.00f, 1.00f)]
            public float weight;
        }
    }
}
