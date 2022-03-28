using UnityEngine;


namespace Objects.Trigger
{
   using Objects.Callback;
   using UnityEngine;
   
   public class StageTrigger : TriggerObject
   {
      const string PLAYER = "PLAYER_BASE";

      protected override void Awake()
      {
         base.Awake();

         OnTrigger += (other) =>
         {
            if(other.CompareTag(PLAYER))
            {
               ExecuteCallback.Call(this.transform);
            }
         };
      }
   }
}