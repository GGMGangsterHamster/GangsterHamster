using UnityEngine;

namespace Objects.Camrea
{
   public class CameraCalibration : MonoBehaviour
   {
      [Header("보정을 위한 응시 오브젝트")]
      public Transform lookAt;



      private void Awake()
      {
         Quaternion.LookRotation(lookAt.position, transform.up);
      }
   }
}