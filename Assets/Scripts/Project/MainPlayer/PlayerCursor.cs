using DDCore;
using UnityEngine;

namespace ProjectBubble.MainPlayer
{
    public class PlayerCursor : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Transform _cursorTransform;
        [SerializeField] private Transform _cursorShadowTransform;

        private void LateUpdate()
        {
            transform.position = Util.GetMouseWorldPosition();
          
            _cursorTransform.Rotate(new Vector3(0, 0, Time.deltaTime * _rotationSpeed));
            _cursorShadowTransform.Rotate(new Vector3(0, 0, Time.deltaTime * _rotationSpeed));
        }
    }
}
