using DDCore;
using UnityEngine;

namespace ProjectBubble.MainPlayer
{
    public class PlayerCursorPointer : MonoBehaviour
    {
        [SerializeField] private float _offset = 2;
        [SerializeField] private GameObject _target;
        [SerializeField] private Transform _arrow;
        [SerializeField] private Transform _arrowShadow;
        private void LateUpdate()
        {
            Vector3 direction = (Util.GetMouseWorldPosition() - transform.position).normalized;
            Quaternion rotation = Util.GetAngle(direction);
            transform.position = _target.transform.position + direction * _offset;
            _arrow.rotation = rotation;
            _arrowShadow.rotation = rotation;
        }
    }
}
