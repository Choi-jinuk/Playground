using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    private PoolUtil<SoundData> _soundPool = new PoolUtil<SoundData>();
    private List<SoundData> _removeList = new List<SoundData>();
    private AudioSource _bgmSource;
    private AudioSource _nextBgm;

    private AudioSource _sfxSource;

    public bool MuteBGM => _bgmSource.mute;
    public bool MuteSfx => _sfxSource.mute;

    private float _bgmVolume = 1.0f;
    private float _sfxVolume = 1.0f;

    public float BgmVolume
    {
        get => _bgmVolume;
        set
        {
            if (_bgmVolume == value)
                return;
            _bgmVolume = value;
            if (_bgmSource)
            {
                _bgmSource.volume = _bgmVolume;
                _bgmSource.mute = _bgmVolume == 0;
            }
        }
    }
    public float SfxVolume
    {
        get => _sfxVolume;
        set
        {
            if (_sfxVolume == value)
                return;
            _sfxVolume = value;
            if (_sfxSource)
            {
                _sfxSource.volume = _sfxVolume;
                _sfxSource.mute = _sfxVolume == 0;
            }
        }
    }

    private CancellationTokenSource _changeBgmToken;

    public void Init()
    {
        _sfxSource = SelfObject.AddComponent<AudioSource>();
        _sfxSource.priority = 0;
        _sfxSource.volume = PlayerPrefs.GetFloat(GlobalString.EffectVolume, 1f);

        _bgmSource = SelfObject.AddComponent<AudioSource>();
        _bgmSource.priority = 1;
        _bgmSource.volume = PlayerPrefs.GetFloat(GlobalString.BgmVolume, 1f);
        _bgmSource.loop = true;

        _nextBgm = SelfObject.AddComponent<AudioSource>();
        _nextBgm.priority = 2;
        _nextBgm.volume = 0f;
        _nextBgm.loop = true;
        
        _soundPool.Init(30);
        
        AudioSettings.OnAudioConfigurationChanged -= OnAudioConfigurationChanged;
        AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
    }
    private void OnAudioConfigurationChanged(bool deviceWasChanged)
    {   //블루투스 전환 혹은 홈 전환 시 소리가 끊기지 않도록 추가
        if (deviceWasChanged)
        {
            var config = AudioSettings.GetConfiguration();
            AudioSettings.Reset(config);
        }
        if (_bgmSource != null)
            _bgmSource.Play();
    }
    public void Play(string name)
    {
        if (_sfxSource == null)
            return;
        var clip = _GetClipByName(name);
        if (clip == null)
        {
            DebugManager.LogError($"AudioClip is Null ({name})");
            return;
        }

        var data = _soundPool.New();
        data.Clip = clip;
        data.Key = name;
        data.Time = clip.length;
        
        _sfxSource.PlayOneShot(clip);
    }

    public async void PlayBGM(string name)
    {
        if (!_bgmSource || !_nextBgm)
            return;
        if (_bgmSource.clip == null)
        {
            _bgmSource.clip = _GetClipByName(name);
            _bgmSource.Play();
            return;
        }
        
        if (_bgmSource.name.Equals(name))
        {
            return;
        }

        _nextBgm.clip = _GetClipByName(name);
        _nextBgm.mute = false;

        _changeBgmToken?.Cancel();
        _changeBgmToken = new CancellationTokenSource();
        await _ChangeBGM();
    }

    private async UniTask _ChangeBGM()
    {
        var changeTime = 3f;
        var elapsedTime = 0f;
        while (elapsedTime < changeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            var offset = elapsedTime / changeTime;
            _bgmSource.volume = BgmVolume * (1 - offset);
            _nextBgm.volume = BgmVolume * offset;

            await UniTask.Yield();
            if (_changeBgmToken.IsCancellationRequested)
                return;
        }

        AddressableManager.Release(_bgmSource.clip);
        _bgmSource.clip = _nextBgm.clip;
        _bgmSource.volume = _nextBgm.volume;
        _nextBgm.clip = null;
        _nextBgm.volume = 0f;
        _nextBgm.mute = true;

        _changeBgmToken.Dispose();
        _changeBgmToken = null;
    }

    AudioClip _GetClipByName(string name)
    {
        return AddressableManager.LoadImmediately<AudioClip>(name, false);
    }

    private async void Update()
    {
        await UniTask.WaitUntil(()=>_soundPool.usingList == null || _soundPool.usingList.Count == 0);
        await UniTask.WaitForSeconds(0.1f);

        _removeList.Clear();
        for (int i = 0; i < _soundPool.usingList.Count; i++)
        {
            var data = _soundPool.usingList[i];
            data.Time -= 0.1f;
            if (data.Time <= 0f)
            {
                _removeList.Add(data);
            }
        }

        if (_removeList.Count > 0)
        {
            foreach (var data in _removeList)
            {
                AddressableManager.Release(data.Clip);
                _soundPool.Remove(data);
            }
        }
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(GlobalString.BgmVolume, _bgmVolume);
        PlayerPrefs.SetFloat(GlobalString.EffectVolume, _sfxVolume);
        PlayerPrefs.Save();
    }

    public void Clear()
    {
        if (_soundPool.usingList != null && _soundPool.usingList.Count > 0)
        {
            foreach (var data in _soundPool.usingList)
            {
                AddressableManager.Release(data.Clip);
            }
            _soundPool.Clear();
        }

        if (_bgmSource != null && _bgmSource.clip != null)
        {
            AddressableManager.Release(_bgmSource.clip);
            _bgmSource.clip = null;
        }
    }
}
