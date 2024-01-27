using Assets.Scripts.Core.Data.Services;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace Assets.Scripts.Core.Utils.AudioService
{
    public class AudioService : MonoBehaviour
    {
        private ISaveService _saveService;
        private RandomProvider _random;
        private Dictionary<SoundType, AudioClipSource> _soundCollection;
        private AudioSource _musicSource;
        private AudioSource _uiSource;
        private ReactiveProperty<bool> _soundOn;
        private ReactiveProperty<bool> _musicOn;

        public IReadOnlyReactiveProperty<bool> SoundOn => _soundOn;
        public IReadOnlyReactiveProperty<bool> MusicOn => _musicOn;

        [Inject]
        public void Construct(ISaveService saveService, ConfigsStorage configStorage, RandomProvider random)
        {
            _saveService = saveService;
            _random = random;

            _soundCollection = new Dictionary<SoundType, AudioClipSource>();
            foreach (var data in configStorage.SoundDataList)
                _soundCollection[data.Type] = new AudioClipSource(data.SoundList);

            _soundOn = new ReactiveProperty<bool>().AddTo(this);
            _musicOn = new ReactiveProperty<bool>().AddTo(this);
        }

        public void PlayUISound(SoundType soundType)
        {
            if (_soundCollection.TryGetValue(soundType, out var source))
            {
                var clip = source.GetRandom(_random);
                if (clip != null)
                    _uiSource.PlayOneShot(clip);
            }
        }

        public void PlayMusic()
        {
            if (_soundCollection.TryGetValue(SoundType.BGMusic, out var source))
            {
                var clip = source.GetRandom(_random);
                if (clip != null)
                {
                    _musicSource.clip = clip;
                    _musicSource.Play();
                }
            }
        }

        public void SwitchSound(bool isOn)
        {
            _soundOn.Value = isOn;
            SaveSoundSettings();
        }

        public void SwitchMusic(bool isOn)
        {
            _musicOn.Value = isOn;
            SaveSoundSettings();
        }

        private void SaveSoundSettings()
        {
            var save = _saveService.GetSettingsSave();
            save.MusicOn = _musicOn.Value;
            save.SoundOn = _soundOn.Value;
            _saveService.Save(save);
        }

        private void Start()
        {
            var save = _saveService.GetSettingsSave();

            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.priority = 0;
            _musicOn.Subscribe(on => _musicSource.mute = !on).AddTo(this);
            _musicOn.Value = save.MusicOn;

            _uiSource = gameObject.AddComponent<AudioSource>();
            _uiSource.loop = false;
            _uiSource.priority = 0;
            _soundOn.Subscribe(on => _uiSource.mute = !on).AddTo(this);
            _soundOn.Value = save.SoundOn;

            DontDestroyOnLoad(gameObject);
        }
    }
}