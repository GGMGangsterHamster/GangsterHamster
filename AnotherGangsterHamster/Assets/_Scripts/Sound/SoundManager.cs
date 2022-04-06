using System.Collections.Generic;
using _Core;
using UnityEngine;

namespace Sound
{
   public class SoundManager : MonoSingleton<SoundManager>
   {
      private List<AudioSource> _pool = new List<AudioSource>();

      protected override void Awake()
      {
         base.Awake();
         GenericPool.Instance.AddManagedObject<AudioSource>();
      }

      private void Start()
      {
         Debug.Log(GenericPool.Instance.Get<AudioSource>().name);
      }
   }
}