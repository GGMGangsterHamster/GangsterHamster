using UnityEngine;

namespace Stages.Management
{   
   public class Checkpoint : MonoBehaviour
   {
      const string PLYAER = "PLAYER_BASE";

      private void OnTriggerEnter(Collider other)
      {
         if (other.CompareTag(PLYAER))
         {
            StageManager.Instance.ActivateCheckpoint(gameObject.name);
         }
      }
   }
}