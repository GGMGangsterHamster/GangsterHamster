using Characters.Player;
using UnityEngine;


namespace Objects.Interaction
{   
   // 메인 카메라에서 Ray 발사함
   public class InteractionRaycaster : MonoBehaviour
   {
      // 현제 상호작용 가능한 오브젝트
      private Interactable _currentObject = null;
      private Transform _mainCam = null;
      private Transform MainCam
      {
         get
         {
            if(_mainCam == null)
               _mainCam = Camera.main.transform;

            return _mainCam;
         }
      }

      private void FixedUpdate()
      {
         Interactable target = FireRay();


         // 상호작용 가능한 오브젝트가 없는 경우
         if(target == null || !target.canInteractByPlayer)
         {
            _currentObject?.DeFocus();
            _currentObject = null;
            InteractionManager.Instance.ClearInteraction();
            return;
         }

         // 같은 오브젝트인 경우
         if(target == _currentObject) return;

         // 새로운 상호작용한 오브젝트를 찾은 경우
         if(_currentObject != null)
         {
            _currentObject.DeFocus();
         }

         _currentObject = target;
         _currentObject?.Focus();
         InteractionManager.Instance.SetInteraction(target);
      }

      private Interactable FireRay()
      {
         Transform target = null;
         if (UnityEngine.Physics.Raycast(MainCam.position,
                                         MainCam.forward,
                                         out RaycastHit hit,
                                         PlayerValues.InteractionMaxDistance))
         {
            target = hit.transform;
         }

         return target?.GetComponent<Interactable>();
      }

   }
}