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
                                          (source) => {
                                             source.playOnAwake = false;
                                          }
         );

         _audioDictionary       = new Dictionary<string, AudioClip>();
         _curPlayingAudioSource = new Dictionary<string, AudioSource>();

         Resources.LoadAll<AudioClip>(soundEffectPath)
                  .ToList()
                  .ForEach(e => {
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
         if(!_audioDictionary.ContainsKey(name))
         {
            Logger.Log($"Cannot find audio {name}", LogLevel.Error);
            return;
         }

         if (_curPlayingAudioSource[name] != null &&
             _curPlayingAudioSource[name].isPlaying && doNotPlayIfAlreadyPlaying)
            return;

         AudioSource source = GenericPool
                                 .Instance
                                 .Get<AudioSource>(e => !e.isPlaying);
         source.gameObject.SetActive(true);
         source.clip   = _audioDictionary[name];
         source.volume = GlobalVolume; // FIXME: TEMP
         source.Play();
         _curPlayingAudioSource[name] = source;
      }
   }
}