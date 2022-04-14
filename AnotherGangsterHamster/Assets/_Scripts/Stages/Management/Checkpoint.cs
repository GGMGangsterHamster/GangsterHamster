using UnityEngine;

namespace Stages.Management
{   
   public class Checkpoint : MonoBehaviour
   {
      const string PLYAER = "PLAYER";

      private void OnTriggerEnter(Collider other)
      {
         if (other.CompareTag(PLYAER))
         {
            StageManager.Instance.ActivateCheckpoint(gameObject.name);
         }
      }
   }
}