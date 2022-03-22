using System;
using Objects.Callback;
using Objects.Interactable.Management;
using Objects.UI.Management;
using UnityEngine;

namespace Objects.Interactable
{

   public class Door : Interactable
   {
      [SerializeField] private GameObject _doorObject;
      [SerializeField] private float _defaultDoorStayOpenDuration = 3.0f;
      [SerializeField] private Transform[] _uiPositions = new Transform[2];


      /// <summary>
      /// 문을 엽니다<br/>
      /// 알아서 Release 해 주니 호출할 필요 없음<br/>
      /// Callback 이 null 이 아니라면 문이 자동으로 닫히지 않음
      /// </summary>
      public override void Interact(Action callback = null)
      {
         _doorObject.SetActive(false);
         if (callback == null)
            Invoke(nameof(Release), _defaultDoorStayOpenDuration); // 오 이런

         FloatingUIManager.Instance.DisableUI();


         callback?.Invoke();
         ExecuteCallback.Call(this.transform);
      }

      public override void Release() // 문을 닫음
      {
         _doorObject.SetActive(true);
      }

      public override void Collision(GameObject collision, Action callback = null) { }

      public override void Focus(Action callback = null)
      {
         if (!_doorObject.activeSelf) return;

         FloatingUIManager.Instance.KeyHelper(KeyCode.E,
                  "를 눌러 문을 여세요.",
                  GameManager.Instance.FindClosestPosition(_uiPositions));
      }

      public override void DeFocus(Action callback = null)
      {
         FloatingUIManager.Instance.DisableUI();
      }
   }
}
