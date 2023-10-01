
using DDCore;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class VFX : MonoBehaviour
    {
        [SerializeField] private bool _isRandom;

        [Header("ScreenShake")]
        [SerializeField] private bool _screenshake;
        [SerializeField] private int _screenshakeStrength;
        [SerializeField] private float _screenshakeDuration;

        [Header("HitStop")]
        [SerializeField] private bool _hitStop;
        [SerializeField] private int _hitStopFrameCount;

        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private GameObject[] _prefabs;

        public void Play(Vector3 worldPosition)
        {
            if (_isRandom)
            {
                if (_prefabs.Length > 0)
                {
                    int prefabIndex = Random.Range(0, _prefabs.Length);
                    GameObject prefab = _prefabs[prefabIndex];
                    GameObject instance = Instantiate(prefab, worldPosition, prefab.transform.rotation);
                    instance.transform.position = worldPosition;
                }

                if (_audioClips.Length > 0)
                {
                    int audioClipIndex = Random.Range(0, _audioClips.Length);
                    AudioClip audioClip = _audioClips[audioClipIndex];
                    AudioManager.PlaySound(audioClip);
                }
            }
            else
            {
                for (int i = 0; i < _prefabs.Length; i++)
                {
                    GameObject prefab = _prefabs[i];
                    GameObject instance = Instantiate(prefab, worldPosition, prefab.transform.rotation);
                    instance.transform.position = worldPosition;
                }

                for (int i = 0; i < _audioClips.Length; i++)
                {
                    AudioClip audioClip = _audioClips[i];
                    AudioManager.PlaySound(audioClip);
                }
            }

            if (_screenshake)
            {
                MainCamera.Screenshake(_screenshakeStrength, _screenshakeDuration);
            }

            if (_hitStop)
            {
                VFXUtil.DoHitStop(_hitStopFrameCount);
            }
        }
    }
}
