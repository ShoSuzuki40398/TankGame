using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusicPlayer : SingletonMonoBehaviour<BackgroundMusicPlayer>
{
    // デジベル(dB)変換の式 db = value(db) * Mathf.Log10(volume);

    [Header("Music Settings")]
    public AudioClip musicAudioClip;
    public AudioMixerGroup musicOutput;
    public bool musicPlayOnAwake = true;

    [SerializeField, Range(0, 1), Tooltip("BGMの音量")]
    private float bgmVolume = 0.5f;

    protected AudioSource m_MusicAudioSource;

    public float BgmVolume
    {
        set
        {
            bgmVolume = Mathf.Clamp01(value);
            m_MusicAudioSource.volume = bgmVolume;
        }
        get
        {
            return bgmVolume;
        }
    }

    protected override void Init()
    {
        // BGM設定
        m_MusicAudioSource = gameObject.AddComponent<AudioSource>();
        m_MusicAudioSource.clip = musicAudioClip;
        m_MusicAudioSource.outputAudioMixerGroup = musicOutput;
        m_MusicAudioSource.loop = true;
        BgmVolume = bgmVolume;

        if (musicPlayOnAwake)
        {
            m_MusicAudioSource.time = 0;
            m_MusicAudioSource.Play();
        }
    }

    /// <summary>
    /// BGM変更
    /// </summary>
    /// <param name="clip"></param>
    public void ChangeMusic(AudioClip clip)
    {
        musicAudioClip = clip;
        m_MusicAudioSource.clip = clip;
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public void Play()
    {
        PlayJustMusic();
    }

    /// <summary>
    /// 最初からBGN再生
    /// </summary>
    private void PlayJustMusic()
    {
        m_MusicAudioSource.time = 0f;
        m_MusicAudioSource.Play();
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void Stop()
    {
        StopJustMusic();
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    private void StopJustMusic()
    {
        m_MusicAudioSource.Stop();
    }

    /// <summary>
    /// ミュート
    /// </summary>
    public void Mute()
    {
        MuteJustMusic();
    }


    /// <summary>
    /// ミュート
    /// </summary>
    private void MuteJustMusic()
    {
        m_MusicAudioSource.volume = 0f;
    }


    /// <summary>
    /// ミュート解除
    /// </summary>
    public void Unmute()
    {
        UnmuteJustMustic();
    }

    /// <summary>
    /// ミュート解除
    /// </summary>
    public void UnmuteJustMustic()
    {
        m_MusicAudioSource.volume = bgmVolume;
    }

    /// <summary>
    /// 徐々に音量変更
    /// </summary>
    /// <param name="source"></param>
    /// <param name="finalVolume"></param>
    /// <param name="fadeTime"></param>
    /// <returns></returns>
    protected IEnumerator VolumeFade(AudioSource source, float finalVolume, float fadeTime)
    {
        float volumeDifference = Mathf.Abs(source.volume - finalVolume);
        float inverseFadeTime = 1f / fadeTime;

        while (!Mathf.Approximately(source.volume, finalVolume))
        {
            float delta = Time.deltaTime * volumeDifference * inverseFadeTime;
            source.volume = Mathf.MoveTowards(source.volume, finalVolume, delta);
            yield return null;
        }
        source.volume = finalVolume;
    }
}
