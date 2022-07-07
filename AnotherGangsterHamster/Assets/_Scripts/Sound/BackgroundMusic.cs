using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���ϰ� �ӽ÷� ������ ��ũ��Ʈ
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
        audio.volume = value;
    }
}
