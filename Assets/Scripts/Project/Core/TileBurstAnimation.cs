
using DDCore;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class TileBurstAnimation : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private float _elapsedTime;
        [SerializeField] private float _startSquareScale;
        [SerializeField] private Sprite _squareSprite;
        public float ArcHeight { get; set; }
        public Action OnComplete { get; set; } 
        public Vector3 StartPosition { get; set; }
        public Vector3 TargetPosition { get; set; }
        private void Start()
        {
            const float Min_Arc_Height = 3;
            const float Max_Arc_Height = 10;
    
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            ArcHeight = UnityEngine.Random.Range(Min_Arc_Height, Max_Arc_Height);
            StartCoroutine(ArcRoutine());
        }

        private IEnumerator ArcRoutine()
        {
            const float Arc_Speed = 2.25f;
            float rotation = UnityEngine.Random.Range(-360f, 360f);
            while(_elapsedTime < 1f)
            {
                _elapsedTime += Time.deltaTime * Arc_Speed;
                float startHeight = 0;
                float midHeight = ArcHeight;
                float endHeight = 0;

                float h1 = Mathf.Lerp(startHeight, midHeight, _elapsedTime);
                float h2 = Mathf.Lerp(midHeight, endHeight, _elapsedTime);
                float h = Mathf.Lerp(h1, h2, _elapsedTime);

                transform.position = Vector3.Lerp(StartPosition, TargetPosition, _elapsedTime);
                _spriteRenderer.transform.localPosition = new Vector3(0, h, 0);
                _spriteRenderer.transform.Rotate(new Vector3(0, 0, rotation * Time.deltaTime));
                yield return null;
            }

            Vector3 startScale  = new Vector3(_startSquareScale, _startSquareScale, 0);
            Vector3 endScale = Vector3.one;

            _spriteRenderer.sprite = _squareSprite;
            _spriteRenderer.transform.rotation = Quaternion.identity;
            _spriteRenderer.transform.localScale = startScale;
            _elapsedTime = 0f;
            while(_elapsedTime < 1f)
            {
                _elapsedTime += Time.deltaTime * Arc_Speed;
                float easedTime = Easing.Calculate(_elapsedTime, EaseType.Out_Cubic);
                _spriteRenderer.transform.localScale = Vector3.Lerp(startScale, endScale, easedTime);
                yield return null;
            }

            OnComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}
