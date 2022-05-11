using Effects.Fullscreen;
using Stages.Management;
using UnityEngine;

namespace Characters.Player
{   
   public class OnDeadEvent : MonoBehaviour, ICallbackable
   {
      public Fade    fade              = null;
      public float   fadeoutDuration   = 2.0f;

      public void Invoke(object param)
      {
         fade.FadeOut(fadeoutDuration, () => {
            StageManager.Instance.Reload();
         });
      }
   }
}