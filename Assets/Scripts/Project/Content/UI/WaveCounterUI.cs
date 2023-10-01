using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using ProjectBubble.Core;
using DDCore;
using System.Collections;

namespace ProjectBubble.Content.UI
{
    public class WaveCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _waveTmp;
        [SerializeField] private RectTransform _rootTransform;
        private void Start()
        {
            WaveManager.OnWaveStart += UpdateUI;
        }

        private void OnDestroy()
        {
            WaveManager.OnWaveStart -= UpdateUI;
        }

        private void UpdateUI(int wave)
        {
 
            StartCoroutine(Routine());
            IEnumerator Routine()
            {
               
                float elapsedTime = 0.0f;
                const float Speed = 2f;
                _rootTransform.localScale = Vector3.zero;
                _waveTmp.text = $"Wave {wave}";
                _waveTmp.gameObject.SetActive(true);

                while (elapsedTime <= 1f)
                {
                    elapsedTime += Time.deltaTime * Speed;
                    float easedTime = Easing.Calculate(elapsedTime, EaseType.In_Out_Cubic);
                    _rootTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, easedTime);
                    yield return null;
                }
          
                yield return new WaitForSeconds(1.5f);
                elapsedTime = 0f;
                while (elapsedTime <= 1f)
                {
                    elapsedTime += Time.deltaTime * Speed;
                    float easedTime = Easing.Calculate(elapsedTime, EaseType.In_Out_Cubic);
                    _rootTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, easedTime);
                    yield return null;
                }
                _waveTmp.gameObject.SetActive(false);
            }
        }
    }
}
