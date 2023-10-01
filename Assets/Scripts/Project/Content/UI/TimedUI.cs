using System.Collections;
using UnityEngine;

namespace ProjectBubble.Content.UI
{
    public class TimedUI : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private GameObject _target;
        private void Start()
        {
            StartCoroutine(Routine());
            IEnumerator Routine()
            {
                yield return new WaitForSeconds(_duration);
                _target.gameObject.SetActive(false);
            }
        }
    }
}
