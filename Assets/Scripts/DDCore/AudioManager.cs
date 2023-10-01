using UnityEngine;
using UnityEngine.Audio;

namespace DDCore
{
    public class AudioManager : MonoBehaviour
    {
        private const int MAX_AUDIO_SOURCE_COUNT = 20;
        private AudioSource _musicSource;
        private AudioSource[] _audioSources;
        private static float _soundVolume = 1f;
        private static float _musicVolume = 1f;
        private static AudioManager _instance;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private float _maxPitchVariation = 0.05f;
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
            _musicSource.outputAudioMixerGroup = _audioMixer.outputAudioMixerGroup;
            _musicSource.loop = true;
            for (int i = 0; i < _audioSources.Length; i++)
            {
    
                _audioSources[i] = gameObject.AddComponent<AudioSource>();
                _audioSources[i].outputAudioMixerGroup = _audioMixer.outputAudioMixerGroup;
            }

            DontDestroyOnLoad(gameObject);
        }

        public static void PlaySound(AudioClip audioClip)
        {
            AudioManager audioManager = _instance;
            AudioSource targetSource = null;
            for (int i = 0; i < audioManager._audioSources.Length; i++)
            {
                AudioSource audioSource = audioManager._audioSources[i];
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
                for (int i = 0; i < audioManager._audioSources.Length; i++)
                {
                    AudioSource audioSource = audioManager._audioSources[i];
                    if (audioSource.time > oldestTime)
                        continue;
                    oldestTime = audioSource.time;
                    targetSource = audioSource;
                }
            }

            targetSource.volume = _soundVolume;
            targetSource.clip = audioClip;
            targetSource.pitch = 1f + Random.Range(-audioManager._maxPitchVariation, audioManager._maxPitchVariation);
            targetSource.Play();
        }

        public static void PlayMusic(AudioClip audioClip)
        {
            AudioManager audioManager = _instance;
            audioManager._musicSource.volume = _musicVolume;
            audioManager._musicSource.clip = audioClip;
            audioManager._musicSource.Play();
        }

        public static void SetSoundVolume(float volume)
        {
            AudioManager audioManager = _instance;
            _soundVolume = volume;
  
            for(int i = 0; i < audioManager._audioSources.Length; i++)
            {
                AudioSource audioSource = audioManager._audioSources[i];
                audioSource.volume = volume;
            }
        }

        public static float GetSoundVolume()
        {
            return _soundVolume;
        }

        public static void SetMusicVolume(float volume)
        {
            AudioManager audioManager = _instance;
            _musicVolume = volume;
            audioManager._musicSource.volume = volume;
        }

        public static float GetMusicVolume()
        {
            return _musicVolume;
        }
    }
}