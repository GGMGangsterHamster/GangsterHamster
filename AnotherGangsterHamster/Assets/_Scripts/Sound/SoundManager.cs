using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using _Core;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Sound
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private Dictionary<string, AudioClip> _audioDictionary;
        private Dictionary<string, AudioSource> _curPlayingAudioSource;
        public string soundEffectPath = "Audio/SoundEffect/";
        public float GlobalVolume { get; set; } = 0.8f;

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            GenericPool
               .Instance
               .AddManagedObject<AudioSource>(this.transform,
                                             (source) =>
                                             {
                                                 source.playOnAwake = false;
                                             }
            );

            _audioDictionary = new Dictionary<string, AudioClip>();
            _curPlayingAudioSource = new Dictionary<string, AudioSource>();

            Resources.LoadAll<AudioClip>(soundEffectPath)
                     .ToList()
                     .ForEach(e =>
                     {
                         _audioDictionary.Add(e.name, e);
                         _curPlayingAudioSource.Add(e.name, null);
                     }
            );
        }

        /// <summary>
        /// 오디오를 플레이 합니다.
        /// </summary>
        /// <param name="name"></param>
        public void Play(string name, bool doNotPlayIfAlreadyPlaying = false)
        {
            if (!_audioDictionary.ContainsKey(name))
            {
                Logger.Log($"Cannot find audio {name}", LogLevel.Error);
                return;
            }

            //if (_curPlayingAudioSource.ContainsKey(name))
            //{
            //    AudioSource sameSource = _curPlayingAudioSource[name];

            //    if (sameSource != null && sameSource.gameObject.activeSelf)
            //        sameSource.Stop();
            //}

            if (_curPlayingAudioSource[name] != null &&
                _curPlayingAudioSource[name].isPlaying &&
                doNotPlayIfAlreadyPlaying)
                return;

            AudioSource source = GenericPool
                                    .Instance
                                    .Get<AudioSource>(e => !e.isPlaying);
            source.gameObject.SetActive(true);
            source.clip = _audioDictionary[name];
            source.volume = GlobalVolume; // FIXME: TEMP
            source.Play();
            _curPlayingAudioSource[name] = source;
        }

        public void Stop(string name)
        {
            if (_curPlayingAudioSource.ContainsKey(name))
            {
                AudioSource source = _curPlayingAudioSource[name];

                if (source != null && source.gameObject.activeSelf)
                    source.Stop();
            }
            else
                Logger.Log($"SoundManager > Cannot found key: {name}",
                    LogLevel.Error);
        }

        public void StopAll()
        {
            _curPlayingAudioSource
                .Values
                .ToList()
                .ForEach(x => {
                    if(x != null && x.gameObject.activeSelf && x.isPlaying)
                        x.Stop();
                });
        }

        public void SetSound(float volume)
        {
            GlobalVolume = volume;

            if (_curPlayingAudioSource != null &&
                _curPlayingAudioSource.Count != 0)
            {
                foreach (AudioSource source in _curPlayingAudioSource.Values)
                {
                    if (source != null && source.gameObject.activeSelf)
                        source.volume = volume;
                }
            }
        }

        public void MuteSound(bool status)
        {
            _curPlayingAudioSource
                .Values
                .ToList()
                .ForEach(x => {
                    if (x != null && x.gameObject.activeSelf)
                        x.mute = status;
                });
        }

        public void PauseSound(bool pause)
        {
            _curPlayingAudioSource
                .Values
                .ToList()
                .ForEach(x => {
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