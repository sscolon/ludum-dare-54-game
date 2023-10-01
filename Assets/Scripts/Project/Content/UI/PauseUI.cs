using ProjectBubble.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectBubble.Content.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootParent;
        private void Start()
        {
            PauseManager.OnPause += ShowUI;
            PauseManager.OnUnPause += HideUI;
        }

        private void OnDestroy()
        {
            PauseManager.OnPause -= ShowUI;
            PauseManager.OnUnPause -= HideUI;
        }

        private void ShowUI()
        {
            //VFX later.
            _rootParent.gameObject.SetActive(true);
        }

        private void HideUI()
        {
            //VFX later
            _rootParent.gameObject.SetActive(false);
        }

        public void Resume()
        {
            PauseManager.UnPause();
        }
    }
}
