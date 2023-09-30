using UnityEngine;

namespace ProjectBubble.Core
{
    [CreateAssetMenu(menuName = "ProjectBubble/Loot")]
    public class Loot : ScriptableObject
    {
        [SerializeField] private int _count;
        [SerializeField] private GameObject[] _prefabs;
        public void Instantiate(Vector3 position)
        {
            const float Max_Speed = 10;
            for (int i = 0; i < _count; i++)
            {
                GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];
                if (prefab == null)
                    continue;

                GameObject instance = GameObject.Instantiate(prefab, position, prefab.transform.rotation);

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
    }
}
