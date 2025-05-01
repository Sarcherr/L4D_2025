using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 音频管理器
/// </summary>
public class AudioManager : MonoSingleton<AudioManager>
{
    /// <summary>
    /// 音频混音器(只用于调节音量)
    /// </summary>
    private AudioMixer audioMixer;

    /// <summary>
    /// 音效源
    /// </summary>
    private AudioSource soundSource;
    /// <summary>
    /// 音乐源
    /// </summary>
    private AudioSource musicSource;

    /// <summary>
    /// 音效列表
    /// </summary>
    private AudioClip[] soundClips;
    /// <summary>
    /// 音乐列表
    /// </summary>
    private AudioClip[] musicClips;

    protected override void Init()
    {
        audioMixer = Resources.Load<AudioMixer>("Audio/Mixer/AudioMixer");

        soundClips = Resources.LoadAll<AudioClip>("Audio/Sound");
        musicClips = Resources.LoadAll<AudioClip>("Audio/Music");

        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound")[0];
        soundSource.playOnAwake = false;
        soundSource.loop = false;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    /// <summary>
    /// 设置主音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume_Master(float volume)
    {
        audioMixer.SetFloat("Volume_Master", volume);
    }
    /// <summary>
    /// 设置音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume_Sound(float volume)
    {
        audioMixer.SetFloat("Volume_Sound", volume);
    }
    /// <summary>
    /// 设置音乐音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume_Music(float volume)
    {
        audioMixer.SetFloat("Volume_Music", volume);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundName">名称(对应文件名称，无后缀)</param>
    /// <param name="volume">音量</param>
    /// <param name="pitch">音调/速度</param>
    public void PlaySound(string soundName, float volume = 1f, float pitch = 1f)
    {
        AudioClip clip = System.Array.Find(soundClips, x => x.name == soundName);
        if (clip != null)
        {
            soundSource.pitch = pitch;
            soundSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="musicName">名称(对应文件名称，无后缀)</param>
    /// <param name="volume">音量</param>
    /// <param name="pitch">音调/速度</param>
    public void PlayMusic(string musicName, float volume = 1f, float pitch = 1f)
    {
        AudioClip clip = System.Array.Find(musicClips, x => x.name == musicName);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.pitch = pitch;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music not found: " + musicName);
        }
    }
    /// <summary>
    /// 停止音乐
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }
    /// <summary>
    /// 暂停音乐
    /// </summary>
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    /// <summary>
    /// 恢复音乐
    /// </summary>
    public void UnpauseMusic()
    {
        musicSource.UnPause();
    }
}
