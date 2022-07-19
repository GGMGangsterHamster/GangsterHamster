using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조금 급하게 임시로 제작한 스크립트
/// <summary>
/// 수정 가능성 99%
/// 수정 가능성 99%
/// 수정 가능성 99%
/// </summary>

public class BackgroundMusic : Singleton<BackgroundMusic>
{
    private bool isFirst = true;

    private GameObject backgroundMusicPlayer;
    private AudioSource audio;
    public void StartBackgroundMusic()
    {
        if(isFirst)
        {
            isFirst = false;
            backgroundMusicPlayer = new GameObject("BackgroundMusicPlayer");
            MonoBehaviour.DontDestroyOnLoad(backgroundMusicPlayer);
            audio = backgroundMusicPlayer.AddComponent<AudioSource>();

            audio.clip = Resources.Load<AudioClip>("Audio/BackgroundMusic");
            audio.loop = true;
            audio.Play();
        }
    }

    public void SetVolume(float value)
    {
        if(audio != null)
        {
            audio.volume = value;
        }
    }
}
