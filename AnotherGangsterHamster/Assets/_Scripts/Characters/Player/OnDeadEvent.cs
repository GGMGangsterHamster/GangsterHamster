using Effects.Fullscreen;
using Stages.Management;
using UnityEngine;

namespace Characters.Player
{   
   public class OnDeadEvent : MonoBehaviour
   {
      public Fade    fade              = null;
      public float   fadeoutDuration   = 2.0f;

      public void Execute()
      {
         fade.FadeOut(fadeoutDuration, () => {
            StageManager.Instance.Reload();
         });
      }
   }
}