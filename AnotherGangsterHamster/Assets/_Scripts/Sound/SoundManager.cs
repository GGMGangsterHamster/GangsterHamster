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
      // public string ttsPath = "Resources/TTS/*.mp3";
      public string soundEffectPath = "Audio/SoundEffect/";
      public float GlobalVolume { get; set; } = 0.8f;

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

         // Debug.Log(Resources.LoadAll<AudioClip>(ttsPath).Length);
         Resources.LoadAll<AudioClip>(soundEffectPath)
                  .ToList()
                  .ForEach(e => {
                     _audioDictionary.Add(e.name, e);
                     Debug.Log(e.name);
                  }
         );
      }

      // private void Start()
      // {
      //    AudioClip clip = Resources.Load<AudioClip>("machine");
      //    Debug.Log(clip.name);
      //    AudioSource.PlayClipAtPoint(clip, Vector3.zero);
      // }

      // private void St123art()
      // {
      //    string path = Path.Combine(Directory.GetCurrentDirectory(), ttsPath);
      //    // Debug.Log(path);
      //    // return;
      //    Directory.GetFiles(path) // 경로 포함한 이름 가져옴
      //                   .ToList()
      //                   .ForEach(e => {
      //                      StartCoroutine(LoadAudio(e, (audio) => {
      //                         _audioDictionary.Add(
      //                            Path.GetFileName(e),
      //                            audio
      //                         );
      //                         Debug.Log(audio.name);

      //                         Play("Gentoo.mp3");
      //                      }));
      //                   });
      // }

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
         source.volume = GlobalVolume; // FIXME: TEMP
         source.Play();
      }
      
      // TODO: 유틸로 빼야함
      /// <summary>
      /// mp3 파일을 로드합니다.
      /// </summary>
      /// <param name="path">확장자 없는 경로</param>
      /// <param name="callback">로드한 파일을 인자로 전달</param>
      // IEnumerator LoadAudio(string path, Action<AudioClip> callback)
      // {
      //    using (UnityWebRequest req =
      //             UnityWebRequestMultimedia
      //                .GetAudioClip($"file:///{path}",
      //                              AudioType.MPEG))
      //    {
      //       yield return req.SendWebRequest();
      //       if(req.result == UnityWebRequest.Result.Success)
      //       {
      //          Logger.Log($"Loaded {path}");

      //          byte[] arr = req.downloadHandler.data;

      //          float[] samples = new float[arr.Length / 4]; //size of a float is 4 bytes

      //          for (int i = 0; i < samples.Length; i++)
      //          {
      //             if (BitConverter.IsLittleEndian)
      //             {
      //                Array.Reverse(arr, i * 4, 4);
      //             }
      //             samples[i] = BitConverter.ToSingle(arr, i * 4) / 0x80000000;
      //          }

      //          int channels = 1; //Assuming audio is mono because microphone input usually is
      //          int sampleRate = 44100; //Assuming your samplerate is 44100 or change to 48000 or whatever is appropriate

      //          AudioClip clip = AudioClip.Create("ClipName", samples.Length, channels, sampleRate, false);
      //          //AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
      //          callback(clip);
      //       }
      //       else
      //       {
      //          Logger.Log($"Failed to load {path}", LogLevel.Error);
      //       }
      //    }
      // }
   }
}