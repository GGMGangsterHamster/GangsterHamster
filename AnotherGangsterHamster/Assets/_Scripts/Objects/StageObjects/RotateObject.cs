using UnityEngine;

namespace Objects.StageObjects
{
   
   public class RotateObject : MonoBehaviour
   {
      [SerializeField] private float anglePerSecond = 360;

      private void FixedUpdate()
      {
         transform.Rotate(Vector3.up * anglePerSecond * Time.deltaTime, Space.Self);
      }
   }
}