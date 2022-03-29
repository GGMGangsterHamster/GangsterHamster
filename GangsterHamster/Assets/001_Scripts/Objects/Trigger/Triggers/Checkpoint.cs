using Objects.Trigger;
using Stage.Management;
using UnityEngine;

namespace Objects.Checkpoint
{   
   public class Checkpoint : TriggerObject
   {
      protected override void Awake()
      {
         base.Awake();

         OnTrigger += (obj) =>
         {
            if(obj.CompareTag("PLAYER_BASE"))
            {
               StageManager.Instance.ActivateCheckpoint(this.gameObject.name);
            }
         };
      }

   }
}