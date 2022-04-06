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
      private List<AudioSource> _pool = new List<AudioSource>();

      public string ttsPath = "Resources/TTS/*.mp3";

      protected override void Awake()
      {
         base.Awake();
         GenericPool.Instance.AddManagedObject<AudioSource>(transform);

         string path = Path.Combine(Directory.GetCurrentDirectory(), ttsPath);

         Directory.GetFiles(path) // 경로 포함한 이름 가져옴
                        .ToList()
                        .ForEach(e => {
                           StartCoroutine(LoadAudio(e, (audio) => {
                              // TODO: 파일 저장
                           }));
                        });

      }

      public void Play()
      {

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
                     .GetAudioClip($"file:///{path}.mp3",
                                   AudioType.MPEG))
         {
            yield return req.SendWebRequest();
            if(req.result == UnityWebRequest.Result.Success)
            {
               Logger.Log($"Loaded {path}.mp3");
               AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
               callback(clip);
            }
            else
            {
               Logger.Log($"Failed to load {path}.mp3", LogLevel.Error);
            }
         }
      }
   }
}