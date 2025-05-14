using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private List<SoundBase> musicSounds, sfxSounds;
        private AudioSource _musicSource, _sfxSource;

        private bool _soundStatus;
        private bool _sfxStatus;

        public bool SoundStatus
        {
            private set
            {
                _soundStatus = value;
                PlayerPrefs.SetInt(nameof(SoundStatus), Convert.ToInt32(_soundStatus));
            } 
            get => Convert.ToBoolean(PlayerPrefs.GetInt(nameof(SoundStatus)));
        }

        public bool SfxStatus
        {
            private set
            {
                _sfxStatus = value;
                PlayerPrefs.SetInt(nameof(SfxStatus), Convert.ToInt32(_sfxStatus));
            }
            get => Convert.ToBoolean(PlayerPrefs.GetInt(nameof(SfxStatus)));
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            DontDestroyOnLoad(Instance);
            CreateMusicSource();
            CreateAudioSource();
        }

        private void Start()
        {
            PlayMusic(Config.AudioName.MainTheme);
        }

        private void CreateMusicSource()
        {
            Instance._musicSource = new GameObject("Music Source").AddComponent<AudioSource>();
            Instance._musicSource.transform.SetParent(transform);
            Instance._musicSource.playOnAwake = false;
        }

        private void CreateAudioSource()
        {
            Instance._sfxSource = new GameObject("SFX Source").AddComponent<AudioSource>();
            Instance._sfxSource.transform.SetParent(transform);
            Instance._sfxSource.playOnAwake = false;
        }

        private void PlayMusic(Config.AudioName musicName)
        {
            var sounds = (from sound in Instance.musicSounds where sound.Name == musicName select sound).ToList();
            var soundToPlay = sounds[Random.Range(0, sounds.Count)];
            
            Instance._musicSource.clip = soundToPlay.Clip;
            Instance._musicSource.volume = soundToPlay.Volume;
            Instance._musicSource.loop = true;
            Instance._musicSource.Play();
        }

        public void PlaySfx(Config.AudioName sfxName, bool loop = false)
        {
            var sounds = (from sound in Instance.sfxSounds where sound.Name == sfxName select sound).ToList();
            var soundToPlay = sounds[Random.Range(0, sounds.Count)];

            if (!loop)
            {
                Instance._sfxSource.PlayOneShot(soundToPlay.Clip, soundToPlay.Volume);
            }
            else
            {
                Instance._sfxSource.clip = soundToPlay.Clip;
                Instance._sfxSource.volume = soundToPlay.Volume;
                Instance._sfxSource.loop = true;
                Instance._sfxSource.Play();
            }

        }

        public void StopMusic()
        {
            Instance._musicSource.Stop();
        }

        public void StopSfx()
        {
            Instance._sfxSource.Stop();
        }

        public void ToggleSfx(bool state)
        {
            Instance._sfxSource.mute = !Instance._sfxSource.mute;
            SfxStatus = state;

        }

        public void ToggleMusic(bool state)
        {
            Instance._musicSource.mute = !Instance._musicSource.mute;
            SoundStatus = state;
        }
    }
}