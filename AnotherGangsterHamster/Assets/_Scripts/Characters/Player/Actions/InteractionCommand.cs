using _Core.Commands;
using Matters.Gravity;
using Objects.Interaction;
using UnityEngine;

namespace Characters.Player.Actions
{
public class Interaction : Command
   {
      IActionable _actionable;
      GravityAffectedObject _curAtype; // 잡고 있는 Atype
      Collider _curAtypeCollider; // 잡고 있는 Atype 컬라이더
      bool _grep;


      public Interaction(IActionable actionable)
      {
         _actionable = actionable;
         _grep = false;
         _curAtype = null;
      }

      /// <summary>
      /// 플레이어가 잡기 위해 상호작용 하는 경우 
      /// param 이 플레이어 손 transform 이어야 함
      /// </summary>
      /// <param name="param"></param>
      public override void Execute(object param = null)
      {
         _actionable.Interact(handle => {
            switch (InteractionManager.Instance.GetGrep())
            {
               case false: // 잡기
                  // TODO: 판단 추후 수정
                  if (handle.lossyScale.x *
                      handle.lossyScale.y *
                      handle.lossyScale.z > 1.1f)
                  {
                     InteractionManager.Instance.UnGrep();
                     return;
                  }

                  handle.SetParent((param as Transform));

                  #region GetComponent
                  _curAtype = handle.GetComponent<GravityAffectedObject>();
                  _curAtypeCollider = handle.GetComponent<Collider>();
                  Rigidbody rigid = handle.GetComponent<Rigidbody>();
                  #endregion // GetComponent

                  #region 불필요 연산 비활성화
                  if (_curAtype != null) // 중력 비활성화
                     _curAtype.AffectedByGlobalGravity = false;

                  if (rigid != null) // 기존 물리 초기화
                  { 
                     rigid.velocity = Vector3.zero;
                     rigid.angularVelocity = Vector3.zero;
                  }

                  if (_curAtypeCollider != null) // 충돌 비활성화
                     _curAtypeCollider.enabled = false;

                  #endregion // 불필요 연산 비활성화

                  // 좌표 초기화
                  handle.localPosition = Vector3.zero;
                  handle.localRotation = Quaternion.identity;

                  // 잡기 처리
                  InteractionManager.Instance.Grep();

                  break;

               case true: // 놓기
                  handle.SetParent(null);

                  if (_curAtype != null) // 물리 연산 활성화
                     _curAtype.AffectedByGlobalGravity = true;
                  if (_curAtypeCollider != null) // 충돌 활성화
                     _curAtypeCollider.enabled = true;

                  // 잡기 해제
                  InteractionManager.Instance.UnGrep();
                  break;
            }
         });
      }

   }
}