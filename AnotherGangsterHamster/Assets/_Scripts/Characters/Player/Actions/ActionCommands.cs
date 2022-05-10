using _Core.Commands;
using Matters.Gravity;
using Objects.Interaction;
using UnityEngine;

// 플레이어 이동 (점프, 웅크리기, 대쉬) 커멘드 패턴

namespace Characters.Player.Actions
{
   public class CrouchStart : Command
   {
      IActionable _actionable;

      public CrouchStart(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.CrouchStart();
      
   }

   public class CrouchEnd : Command
   {
      IActionable _actionable;

      public CrouchEnd(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.CrouchEnd();

   }

   public class DashStart : Command
   {
      IActionable _actionable;

      public DashStart(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.DashStart();

   }

   public class DashEnd : Command
   {
      IActionable _actionable;

      public DashEnd(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.DashEnd();

   }

   public class Jump : Command
   {
      IActionable _actionable;

      public Jump(IActionable actionable)
               => _actionable = actionable;
      public override void Execute(object param = null)
               => _actionable.Jump();

   }

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
               case false:
                  handle.SetParent((param as Transform));

                  _curAtype = // 중력 받는 오브젝트라면 중력 비활성
                     handle.GetComponent<GravityAffectedObject>();

                  if (_curAtype != null) // 물리 연산 비활성화
                  {
                     _curAtype.AffectedByGlobalGravity = false;
                     handle.GetComponent<Rigidbody>().velocity = Vector3.zero;
                  }

                  _curAtypeCollider = // 컬라이더 체크
                     handle.GetComponent<Collider>();

                  if (_curAtypeCollider != null) // 충돌 비활성화
                     _curAtypeCollider.enabled = false;

                  handle.localPosition = Vector3.zero;
                  InteractionManager.Instance.Grep();
                  break;

               case true:
                  handle.SetParent(null);

                  if (_curAtype != null) // 물리 연산 활성화
                     _curAtype.AffectedByGlobalGravity = true;
                  if (_curAtypeCollider != null)
                     _curAtypeCollider.enabled = true;

                     
                  InteractionManager.Instance.UnGrep();

                  InteractionManager.Instance.UnGrep();
                  break;
            }
         });
      }

   }
}