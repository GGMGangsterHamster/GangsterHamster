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
      public string ttsPath = "Resources/TTS/*.mp3";

      protected override void Awake()
      {
         base.Awake();
         GenericPool
            .Instance
            .AddManagedObject<AudioSource>(transform,
                                          (source) => {
                                             source.playOnAwake = false;
                                          }
         );

         _audioDictionary = new Dictionary<string, AudioClip>();

         string path = Path.Combine(Directory.GetCurrentDirectory(), ttsPath);

         Directory.GetFiles(path) // 경로 포함한 이름 가져옴
                        .ToList()
                        .ForEach(e => {
                           StartCoroutine(LoadAudio(e, (audio) => {
                              _audioDictionary.Add(
                                 Path.GetFileName(e),
                                 audio
                              );

                              Play("Button.mp3");
                           }));
                        });
      }

      /// <summary>
      /// 오디오를 플레이 합니다.
      /// </summary>
      /// <param name="name"></param>
      public void Play(string name)
      {
         if(!_audioDictionary.ContainsKey(name))
         {
            Logger.Log($"Cannot find audio {name}", LogLevel.Error);
            return;
         }

         AudioSource source = GenericPool
                                 .Instance
                                 .Get<AudioSource>(e => !e.isPlaying);
         source.gameObject.SetActive(true);
         source.clip = _audioDictionary[name];
         source.Play();
      }
      
      // TODO: 유틸로 빼야함
      /// <summary>
      /// mp3 파일을 로드합니다.
      /// </summary>
      /// <param name="path">확장자 없는 경로</param>
      /// <param name="callback">로드한 파일을 인자로 전달</param>
      IEnumerator LoadAudio(string path, Action<AudioClip> callback)
      {
         using (UnityWebRequest req =
                  UnityWebRequestMultimedia
                     .GetAudioClip($"file:///{path}",
                                   AudioType.MPEG))
         {
            yield return req.SendWebRequest();
            if(req.result == UnityWebRequest.Result.Success)
            {
               Logger.Log($"Loaded {path}");
               AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
               callback(clip);
            }
            else
            {
               Logger.Log($"Failed to load {path}", LogLevel.Error);
            }
         }
      }
   }
}