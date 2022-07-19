using UnityEngine;
using UnityEngine.UI;
using Weapons.Actions;

namespace Objects.Interaction
{  
   public class InteractionManager : Singleton<InteractionManager>
   {
      IInteractable       _currentActiveInteraction;
      Transform          _currentActiveAtype;
      public Transform currentRaycastHitTrm { get; set; }
      public RaycastHit  currentRaycastHit;

      WeaponManagement _weaponManagement;

      WeaponManagement WM
      {
          get
          {
              if(_weaponManagement == null)
              {
                  _weaponManagement = GameObject.FindObjectOfType<WeaponManagement>();
              }

              return _weaponManagement;
          }
      }

      private bool _grep = false; // 잡기 상태

      public void SetActiveAtype(Transform transform)
      {
         if (!_grep) // 잡기 상태가 아닐 시
         {
            _currentActiveAtype = transform;
         }
      }

      public void SetRaycastHitTrm(RaycastHit hit)
      {
         currentRaycastHitTrm = hit.transform;
         currentRaycastHit = hit;
      }

      public void UnGrep()
      {
         _grep = false; 

         WeaponAction wa = WM.GetCurrentWeaponAction();

         if(wa != null)
         {
            if (!wa.gameObject.activeSelf)
            {
                wa.gameObject.SetActive(true);
                wa.ResetPosiiton();
            }
         }

         ClearActvieAtype();
      }

      public void Grep()
      {
         _grep = true;

         WeaponAction wa = WM.GetCurrentWeaponAction();
         

         if (wa != null)
            wa.gameObject.SetActive(!wa.IsHandleWeapon());
      }

      public bool GetGrep() => _grep;

      public void ClearActvieAtype()
      {
         if (!_grep)
         {
            _currentActiveAtype = null;
            _grep = false;
         }
      }

      public void SetInteraction(IInteractable interactable)
               => _currentActiveInteraction = interactable;

      public void UnSetInteraction(IInteractable interactable)
      {
         if (_currentActiveInteraction == interactable)
            _currentActiveInteraction = null;
      }

      /// <summary>
      /// 상호작용 가능한 오브젝트를 null 로 바꿉니다.
      /// </summary>
      public void ClearInteraction()
      {
          _currentActiveInteraction = null;
      }

      /// <summary>
      /// 상호작용 합니다.<br/>
      /// 들기 가능한 오브젝트가 있다면 듭니다.
      /// </summary>
      public void Interact(System.Action<Transform> onPickup = null)
      {
         if (_currentActiveInteraction != null)
         {
            _currentActiveInteraction.Interact();
         }
         if (_currentActiveAtype != null)
         {
            onPickup?.Invoke(_currentActiveAtype);
         }
      }

   }
}