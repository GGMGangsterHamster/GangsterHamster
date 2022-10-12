using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using _Core;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Audio;

namespace Sound
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] private AudioMixer audioMixer;
        private Dictionary<string, AudioClip> _audioDictionary;
        private Dictionary<string, AudioSource> _curPlayingAudioSource;
        private Dictionary<string, AudioSource> audioDictionary = new Dictionary<string, AudioSource>();

        //public float GlobalVolume { get; set; } = 0.8f;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            
            //GenericPool
            //   .Instance
            //   .AddManagedObject<AudioSource>(this.transform,
            //                                 (source) =>
            //                                 {
            //                                     source.playOnAwake = false;
            //                                 }
            //);

            //_audioDictionary = new Dictionary<string, AudioClip>();
            //_curPlayingAudioSource = new Dictionary<string, AudioSource>();


            //Resources.LoadAll<AudioClip>(soundEffectPath)
            //         .ToList()
            //         .ForEach(e =>
            //         {
            //             _audioDictionary.Add(e.name, e);
            //             _curPlayingAudioSource.Add(e.name, null);
            //         }
            //);
        }

        /// <summary>
        /// 해당 오디오를 추가합니다.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="audioSource"></param>
        public void AddAudioSource(string name, AudioSource audioSource)
        {
            if (!audioDictionary.ContainsKey(name))
            {
                audioDictionary.Add(name, audioSource);
            }
        }

        /// <summary>
        /// 해당 오디오를 플레이합니다.
        /// </summary>
        /// <param name="name"></param>
        public void Play(string name, bool doNotPlayIfAlreadyPlaying = false)
        {
            //if (audioDictionary.ContainsKey(name))
            //{
            //    audioDictionary[name].volume = GlobalVolume;
            //    audioDictionary[name].Play();  
            //}
            //else
            //{
            //    Logger.Log($"Cannot find audio {name}", LogLevel.Error);
            //    return;
            //}

         
            //if (!_audioDictionary.ContainsKey(name))
            //{
            //    Logger.Log($"Cannot find audio {name}", LogLevel.Error);
            //    return;
            //}

            //if (_curPlayingAudioSource.ContainsKey(name))
            //{
            //    AudioSource sameSource = _curPlayingAudioSource[name];

            //    if (sameSource != null && sameSource.gameObject.activeSelf)
            //        sameSource.Stop();
            //}

            //if (_curPlayingAudioSource[name] != null &&
            //    _curPlayingAudioSource[name].isPlaying &&
            //    doNotPlayIfAlreadyPlaying)
            //    return;

            //AudioSource source = GenericPool
            //                        .Instance
            //                        .Get<AudioSource>(e => !e.isPlaying);
            //source.gameObject.SetActive(true);
            //source.clip = _audioDictionary[name];
            //source.volume = GlobalVolume; // FIXME: TEMP
            //source.Play();
            //_curPlayingAudioSource[name] = source;
        }

        /// <summary>
        /// 해당 오디오를 멈춥니다.
        /// </summary>
        /// <param name="name"></param>
        public void Stop(string name)
        {
            if (audioDictionary.ContainsKey(name))
            {
                if (audioDictionary[name] != null && audioDictionary[name].gameObject.activeSelf)
                {
                    audioDictionary[name].Stop();
                }
            }
            else
            {
                Logger.Log($"SoundManager > Cannot found key: {name}",
                    LogLevel.Error);
            }      
        }

        /// <summary>
        /// 모든 오디오를 멈춥니다.
        /// </summary>
        public void StopAll()
        {
            audioDictionary
                .Values
                .ToList()
                .ForEach(x => {
                    if(x != null && x.gameObject.activeSelf && x.isPlaying)
                        x.Stop();
                });
        }

        /// <summary>
        /// 글로벌 볼륨을 설정합니다.
        /// </summary>
        /// <param name="volume"></param>
        public void SetMasterVolume(float volume)
        {
            if (volume <= -25)
            {
                volume = -80;
            }

            audioMixer.SetFloat("MasterVolume", volume);

            
            //GlobalVolume = volume;

            //if (_curPlayingAudioSource != null &&
            //    _curPlayingAudioSource.Count != 0)
            //{
            //    foreach (AudioSource source in _curPlayingAudioSource.Values)
            //    {
            //        if (source != null && source.gameObject.activeSelf)
            //            source.volume = volume;
            //    }
            //}
        }

        public void SetVolume(string name, float volume)
        {
             audioMixer.SetFloat(name, volume);
        }

        /// <summary>
        /// 모든 오디오를 음소거시킵니다.
        /// </summary>
        /// <param name="status"></param>
        public void MuteAll(bool status)
        {
            audioDictionary
                .Values
                .ToList()
                .ForEach(x => {
                    if (x != null && x.gameObject.activeSelf)
                        x.mute = status;
                });
        }

        /// <summary>
        /// 모든 오디오를 일시정지합니다.
        /// </summary>
        /// <param name="pause"></param>
        public void PauseAll(bool pause)
        {
            audioDictionary
                .Values
                .ToList()
                .ForEach(x =>
                {
                    if (x != null && x.gameObject.activeSelf)
                    {
                        if (pause)
                            x.Pause();
                        else
                            x.UnPause();
                    }
                });
        }
    }
}