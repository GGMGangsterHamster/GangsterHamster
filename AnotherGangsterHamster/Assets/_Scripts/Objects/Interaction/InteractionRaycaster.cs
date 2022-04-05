using UnityEngine;


namespace Characters.Player.Interaction
{   
   // 메인 카메라에서 Ray 발사함
   public class InteractionRaycaster : MonoBehaviour
   {
      // 현제 상호작용 가능한 오브젝트
      private Interactable _currentObject = null;
      private Transform _mainCam = null;


      private void FixedUpdate()
      {
         // Interactable target = FireRay();
      }

      // private Interactable FireRay()
      // {
         // Transform target = null;
         // if (UnityEngine.Physics.Raycast())
      // }

   }
}