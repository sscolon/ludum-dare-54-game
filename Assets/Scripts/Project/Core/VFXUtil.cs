using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class VFXUtil : MonoBehaviour
    {
        private static VFXUtil _instance;
        [SerializeField] private Material _spriteWhite;
        [SerializeField] private Material _spriteDefault;
        private void OnEnable()
        {
            _instance = this;
        }

        public static void DoSpriteFlash(GameObject target)
        {
            SpriteRenderer spriteRenderer = target.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.material = _instance._spriteWhite;
            _instance.StartCoroutine(Routine());
            IEnumerator Routine()
            {
                yield return new WaitForSeconds(0.25f);
                spriteRenderer.material = _instance._spriteDefault;
            }
        }

        public static void DoHitStop(int frames)
        {
            _instance.StartCoroutine(Routine());
            IEnumerator Routine()
            {
                Time.timeScale = 0;
                var yield = new WaitForEndOfFrame();
                for(int i = 0; i < frames; i++)
                {
                    yield return yield;
                }
                Time.timeScale = 1;
            }
        }
    }
}
