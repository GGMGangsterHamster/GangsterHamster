using UnityEngine;

namespace Objects.Trigger
{
   public class ResetPlayerPositionTrigger : Trigger
   {
      const string PLAYER = "PLAYER_BASE";
      [SerializeField] Transform _resetTrm;

      public override void OnTrigger(GameObject other)
      {
         if(other.CompareTag("PLAYER_BASE"))
         {
            other.transform.position = _resetTrm.position;
         }

      }
   }
}