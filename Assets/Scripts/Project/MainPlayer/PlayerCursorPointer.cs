using DDCore;
using UnityEngine;

namespace ProjectBubble.MainPlayer
{
    public class PlayerCursorPointer : MonoBehaviour
    {
        [SerializeField] private float _offset = 2;
        [SerializeField] private Transform _arrow;
        [SerializeField] private Transform _arrowShadow;

        public Transform Target { get; set; }
        private void LateUpdate()
        {
            if (Target == null)
                return;
            Vector3 direction = (Util.GetMouseWorldPosition() - transform.position).normalized;
            Quaternion rotation = Util.GetAngle(direction);
            transform.position = Target.transform.position + direction * _offset;
            _arrow.rotation = rotation;
            _arrowShadow.rotation = rotation;
        }
    }
}
