using UnityEngine;

namespace ProjectBubble.Content.Projectiles
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed;
        private void Update()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime);
        }
    }
}
