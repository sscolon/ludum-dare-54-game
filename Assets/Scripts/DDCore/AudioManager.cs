using UnityEngine;
using UnityEngine.Audio;

namespace DDCore
{
    public class AudioManager : MonoBehaviour
    {
        private const int MAX_AUDIO_SOURCE_COUNT = 20;
        private AudioSource _musicSource;
        private AudioSource[] _audioSources;
        private static AudioManager _instance;
        [SerializeField] private float _maxPitchVariation = 0.05f;
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup ambientGroup;

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            if(_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            _audioSources = new AudioSource[MAX_AUDIO_SOURCE_COUNT];
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            for (int i = 0; i < _audioSources.Length; i++)
            {
                _audioSources[i] = gameObject.AddComponent<AudioSource>();
            }

            DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(AudioClip audioClip, float volume = 1f)
        {
            AudioSource targetSource = null;
            for (int i = 0; i < _audioSources.Length; i++)
            {
                AudioSource audioSource = _audioSources[i];
                if (audioSource.clip == audioClip)
                {
                    targetSource = audioSource;
                    break;
                }

                if (audioSource.isPlaying)
                    continue;
                targetSource = audioSource;
            }

            float oldestTime = float.MinValue;
            if (targetSource == null)
            {
                //Find oldest audio source if ran out of sources.
                for (int i = 0; i < _audioSources.Length; i++)
                {
                    AudioSource audioSource = _audioSources[i];
                    if (audioSource.time > oldestTime)
                        continue;
                    oldestTime = audioSource.time;
                    targetSource = audioSource;
                }
            }

            targetSource.volume = volume;
            targetSource.clip = audioClip;
            targetSource.pitch = UnityEngine.Random.Range(-_maxPitchVariation, _maxPitchVariation);
            targetSource.Play();
        }

        public void PlayMusic(AudioClip audioClip)
        {
            _musicSource.clip = audioClip;
            _musicSource.Play();
        }

        public static AudioManager GetInstance()
        {
            return _instance;
        }
    }
}