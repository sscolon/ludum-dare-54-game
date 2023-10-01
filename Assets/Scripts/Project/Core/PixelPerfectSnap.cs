using DDCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class PixelPerfectSnap : MonoBehaviour
    {
        private Vector3 _originalLocal;
        private void Start()
        {
            _originalLocal = transform.localPosition;
        }

        private void LateUpdate()
        {
            Snap();
        }

        private void Snap()
        {
            Vector3 pixelPosition = PixelUtil.PixelRound(transform.root, transform);
            Vector3 pixelLocal = PixelUtil.PixelRound(_originalLocal);
            transform.localPosition = pixelPosition + pixelLocal;
        }
    }
}
