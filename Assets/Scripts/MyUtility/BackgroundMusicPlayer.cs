﻿using System.Collections;
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

    protected override void Awake()
    {
        base.Awake();

        // BGM設定
        m_MusicAudioSource = gameObject.AddComponent<AudioSource>();
        m_MusicAudioSource.clip = musicAudioClip;
        m_MusicAudioSource.outputAudioMixerGroup = musicOutput;
        m_MusicAudioSource.loop = true;
        BgmVolume = bgmVolume;

        if(musicPlayOnAwake)
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

    public void Play()
    {
        PlayJustMusic();
    }

    public void PlayJustMusic()
    {
        m_MusicAudioSource.time = 0f;
        m_MusicAudioSource.Play();
    }

    public void Stop()
    {
        StopJustMusic();
    }

    public void StopJustMusic()
    {
        m_MusicAudioSource.Stop();
    }

    public void Mute()
    {
        MuteJustMusic();
    }

    public void MuteJustMusic()
    {
        m_MusicAudioSource.volume = 0f;
    }

    public void Unmute()
    {
        UnmuteJustMustic();
    }

    public void UnmuteJustMustic()
    {
        m_MusicAudioSource.volume = bgmVolume;
    }

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
