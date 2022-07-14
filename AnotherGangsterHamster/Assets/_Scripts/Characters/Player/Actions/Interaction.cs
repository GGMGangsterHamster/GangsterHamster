using _Core.Commands;
using Matters.Gravity;
using Matters.Velocity;
using Objects.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Characters.Player.Actions
{
   public class Interaction : Command
   {
      IActionable _actionable;
      GravityAffectedObject _curAtype;           // 잡고 있는 Atype
      Rigidbody _curRigid;           // 잡고 있는 Atype Rigidbody

      #region 플레이어와 메인카메라의 Trm
      private Transform _mainCameraTransform;
      private Transform _playerBaseTransform;

      protected Transform MainCameraTransform
      {
         get
         {
            if (_mainCameraTransform == null)
            {
               _mainCameraTransform = Camera.main.transform;
            }

            return _mainCameraTransform;
         }
      }
      protected Transform PlayerBaseTransform
      {
         get
         {
            if (_playerBaseTransform == null)
            {
               _playerBaseTransform = GameObject.FindGameObjectWithTag("PLAYER_BASE").transform;
            }

            return _playerBaseTransform;
         }
      }
      #endregion

      public Interaction(IActionable actionable)
      {
         _curAtype = null;

         SceneManager.sceneLoaded += (s, enumm) =>
         {
            InteractionManager.Instance.UnGrep();
         };

         Execute.AddListener(param =>
         {
            actionable.Interact(handle =>
            {
               switch (InteractionManager.Instance.GetGrep())
               {
                  case false: // 잡기
                     Vector3 gravityDir = GravityManager.GetGlobalGravityDirection();

                     if ((handle.lossyScale.x *
                      handle.lossyScale.y *
                      handle.lossyScale.z > 1.1f ||
                      handle.gameObject.isStatic ||
                      handle.name.CompareTo("Grand") == 0) ||
                      (Vector3.Distance(MainCameraTransform.position, handle.position) > 2.5f))
                     {
                        InteractionManager.Instance.UnGrep();
                        return;
                     }

                     PlayerBaseTransform.GetComponent<FollowGroundPos>().Deactive(handle.gameObject);

                     #region GetComponent
                     _curAtype = handle.GetComponent<GravityAffectedObject>();
                     _curRigid = handle.GetComponent<Rigidbody>();
                     #endregion // GetComponent

                     #region 불필요 연산 비활성화
                     if (_curAtype != null) // 중력 비활성화
                     {
                        _curAtype.AffectedByGlobalGravity = false;
                        _curAtype.SetIndividualGravity(Vector3.zero, 0.0f);
                     }

                     if (_curRigid != null) // 기존 물리 초기화
                     {
                        _curRigid.velocity = Vector3.zero;
                        _curRigid.angularVelocity = Vector3.zero;
                     }

                     #endregion // 불필요 연산 비활성화

                     // 잡기 처리
                     InteractionManager.Instance.Grep();

                     _curAtype.StartCoroutine(GrappingRoutine());

                     break;

                  case true: // 놓기
                     if (_curAtype != null) // 물리 연산 활성화
                        _curAtype.AffectedByGlobalGravity = true;

                     if (_curRigid != null)
                        _curRigid.velocity /= 4.5f;

                     InteractionManager.Instance.UnGrep();
                     break;
               }
            });
         });
      }

      IEnumerator GrappingRoutine()
      {
         float beforeMass = _curRigid.mass;
         _curRigid.mass = 0.001f;
         while (InteractionManager.Instance.GetGrep())
         {
            Vector3 moveDir = MainCameraTransform.position - _curRigid.position + MainCameraTransform.forward * 2;
            Vector3 globalGravityDir = -GravityManager.GetGlobalGravityDirection();

            Vector3 horizontalVec = _curRigid.position - PlayerBaseTransform.position;
            float vertical = Vector3.Dot(MainCameraTransform.forward, globalGravityDir); // 카메라 기준 수직 높이

            if (vertical < -0.8f)
            {
               moveDir = new Vector3(moveDir.x * (globalGravityDir.x == 0 ? 1 : 0), moveDir.y * (globalGravityDir.y == 0 ? 1 : 0), moveDir.z * (globalGravityDir.z == 0 ? 1 : 0));

               moveDir = new Vector3((Mathf.Abs(MainCameraTransform.forward.x) < 0.5f || moveDir.x == 0 ? (PlayerBaseTransform.forward.x - horizontalVec.x) : moveDir.x),
                                     (Mathf.Abs(MainCameraTransform.forward.y) < 0.5f || moveDir.y == 0 ? (PlayerBaseTransform.forward.y - horizontalVec.y) : moveDir.y),
                                     (Mathf.Abs(MainCameraTransform.forward.z) < 0.5f || moveDir.z == 0 ? (PlayerBaseTransform.forward.z - horizontalVec.z) : moveDir.z));
            }

            _curRigid.velocity = moveDir * 20;
            _curRigid.angularVelocity = Vector3.Lerp(_curRigid.angularVelocity, Vector3.zero, 0.5f);
            _curRigid.transform.rotation = Quaternion.Slerp(_curRigid.transform.rotation, Quaternion.LookRotation(PlayerBaseTransform.forward), 0.5f);

            if (Vector3.Distance((MainCameraTransform.position + (MainCameraTransform.forward * 2f)), _curRigid.transform.position) > 2.5f
            || Vector3.Distance(PlayerBaseTransform.position, _curRigid.position) < 0.85f)
            {
               InteractionManager.Instance.UnGrep();

               if (_curAtype != null) // 물리 연산 활성화
                  _curAtype.AffectedByGlobalGravity = true;

               _curRigid.velocity /= 4.5f;
               break;
            }

            yield return null;
         }

         _curRigid.mass = beforeMass;
      }
   }
}