using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
   public class SoundManager : MonoSingleton<SoundManager>
   {
      private List<AudioSource> _pool = new List<AudioSource>();


      protected override void Awake()
      {
         base.Awake();
         
      }
   }
}