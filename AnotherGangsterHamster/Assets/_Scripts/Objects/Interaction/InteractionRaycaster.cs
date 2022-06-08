using Characters.Player;
using UnityEngine;
using UnityEngine.UI;
using Weapons.Actions;

namespace Objects.Interaction
{   
   // 메인 카메라에서 Ray 발사함
   public class InteractionRaycaster : MonoBehaviour
   {
      public const string ATYPE = "ATYPEOBJECT";
        
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

      private WeaponManagement _wm;
      private Lumo _lumo;
      private Gravito _gravito;

      private Lumo lumo
      {
         get
         {
            if(_lumo == null)
            {
               if (_wm == null) _wm = GameObject.FindObjectOfType<WeaponManagement>();
               _lumo = _wm.transform.GetChild(0).GetComponent<Lumo>();
            }
            return _lumo;
         }
      }

      private Gravito gravito
      {
         get
         {
            if (_gravito == null)
            {
               if (_wm == null) _wm = GameObject.FindObjectOfType<WeaponManagement>();
               _gravito = _wm.transform.GetChild(2).GetComponent<Gravito>();
            }
            return _gravito;
         }
      }
        
        // 현제 시아에 들어온 Atype 오브젝트
      private Transform _curAtype = null;

      private void FixedUpdate()
      {
         Interactable target = FireRay();


         // 상호작용 가능한 오브젝트가 없는 경우
         if (target == null || !target.canInteractByPlayer)
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
         bool resetHandleObject = true;
         Transform target = null;

         RaycastHit[] hits = Physics.RaycastAll(MainCam.position, MainCam.forward, PlayerValues.InteractionMaxDistance);

         RaycastHit hit = new RaycastHit();
         float minDist = float.MaxValue;
         
         // 지금 상호작용 할 수 있는 오브젝트를 A와 B 타입 오브젝트로 제한을 뒀음
         foreach(RaycastHit rayCastHit in hits)
         {
             if(rayCastHit.transform.CompareTag("BTYPEOBJECT") || rayCastHit.transform.CompareTag("ATYPEOBJECT"))
             {
                 if(minDist > Vector3.Distance(MainCam.position, rayCastHit.point))
                 {
                     hit = rayCastHit;
                     minDist = Vector3.Distance(MainCam.position, rayCastHit.point);
                 }
             }
         }

         if (minDist != float.MaxValue)
         {
            target = hit.transform;
            
                
            if (target.CompareTag(ATYPE) && !(target == gravito.SticklyTrm() || (lumo.SticklyTrm() != null && target == lumo.SticklyTrm())))
            {
               resetHandleObject = false;
               InteractionManager
                     .Instance
                     .SetActiveAtype(target.transform);
            }
         }

         if (resetHandleObject)
         {
            InteractionManager.Instance.ClearActvieAtype();
         }

         return target?.GetComponent<Interactable>();
      }
   }
}