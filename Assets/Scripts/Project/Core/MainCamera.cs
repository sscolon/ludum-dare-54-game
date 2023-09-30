using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class MainCamera : MonoBehaviour
    {
        [field: SerializeField]
        public Transform Target { get; set; }

        [field: SerializeField]
        public Vector3 TargetOffset { get; set; } = new Vector3(0, 0, -10);

        [field: SerializeField]
        public float TrackingSpeed { get; set; }

        [field: SerializeField]
        public float ProximityMultiplier { get; set; }

        [field: SerializeField]
        public float ProximityThreshold { get; set; }

        private void LateUpdate()
        {
            float maxDistanceDelta = Time.deltaTime * TrackingSpeed;
            transform.position = Vector2.MoveTowards(transform.position, Target.position, maxDistanceDelta);
            transform.position = transform.position + TargetOffset;
        }


    }
}
