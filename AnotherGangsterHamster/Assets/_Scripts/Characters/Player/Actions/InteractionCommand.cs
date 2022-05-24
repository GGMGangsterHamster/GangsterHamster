using _Core.Commands;
using Matters.Gravity;
using Objects.Interaction;
using System.Collections;
using UnityEngine;

namespace Characters.Player.Actions
{
    public class Interaction : Command
    {
        IActionable _actionable;
        GravityAffectedObject _curAtype;           // 잡고 있는 Atype
        Rigidbody _curRigid;           // 잡고 있는 Atype Rigidbody
        Collider _curAtypeCollider;   // 잡고 있는 Atype 컬라이더

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
            _actionable = actionable;
            _curAtype = null;
        }

        /// <summary>
        /// 플레이어가 잡기 위해 상호작용 하는 경우 
        /// param 이 플레이어 손 transform 이어야 함
        /// </summary>
        /// <param name="param"></param>
        public override void Execute(object param = null)
        {
            _actionable.Interact(handle =>
            {
                switch (InteractionManager.Instance.GetGrep())
                {
                    case false: // 잡기
                        Debug.Log(Mathf.Abs(Mathf.Abs(MainCameraTransform.position.y) - Mathf.Abs(handle.position.y)));

                        if ((handle.lossyScale.x *
                            handle.lossyScale.y *
                            handle.lossyScale.z > 1.1f ||
                            handle.gameObject.isStatic ||
                            handle.name.CompareTo("Grand") == 0) || 
                            Mathf.Abs(Mathf.Abs(MainCameraTransform.position.y) - Mathf.Abs(handle.position.y)) > 1.1f)
                        {
                            InteractionManager.Instance.UnGrep();
                            return;
                        }

                        //handle.SetParent((param as Transform));

                        #region GetComponent
                        _curAtype = handle.GetComponent<GravityAffectedObject>();
                        _curAtypeCollider = handle.GetComponent<Collider>();
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
                            //_curRigid.constraints = RigidbodyConstraints.FreezeAll;
                        }

                        // if (_curAtypeCollider != null) // 충돌 비활성화
                        //    _curAtypeCollider.enabled = false;

                        #endregion // 불필요 연산 비활성화

                        // 잡기 처리
                        InteractionManager.Instance.Grep();

                        _curAtype.StartCoroutine(GrappingRoutine());

                        break;

                    case true: // 놓기
                        //handle.SetParent(null);

                        if (_curAtype != null) // 물리 연산 활성화
                            _curAtype.AffectedByGlobalGravity = true;

                        if (_curRigid != null)
                            //_curRigid.constraints = RigidbodyConstraints.None;

                        // if (_curAtypeCollider != null) // 충돌 활성화
                        //    _curAtypeCollider.enabled = true;

                        // 잡기 해제
                        InteractionManager.Instance.UnGrep();
                        break;
                }
            });
        }

        IEnumerator GrappingRoutine()
        {
            while(InteractionManager.Instance.GetGrep())
            {
                Vector3 moveDir = ((PlayerBaseTransform.position + (PlayerBaseTransform.forward * 1.4f) + PlayerBaseTransform.up) - _curRigid.transform.position);
                _curRigid.velocity = moveDir * 20;
                _curRigid.angularVelocity = Vector3.Lerp(_curRigid.angularVelocity, Vector3.zero, 0.5f);
                _curRigid.transform.rotation = Quaternion.Slerp(_curRigid.transform.rotation, Quaternion.LookRotation(PlayerBaseTransform.forward), 0.5f);

                // 거리가 일정 이상으로 멀어지면 잡기 풀기
                if(Vector3.Distance((PlayerBaseTransform.position + (PlayerBaseTransform.forward * 1.4f) + PlayerBaseTransform.up), _curRigid.transform.position) > 1.5f)
                {
                    InteractionManager.Instance.UnGrep();

                    if (_curAtype != null) // 물리 연산 활성화
                        _curAtype.AffectedByGlobalGravity = true;
                    break;
                }

                yield return null;
            }
        }
    }
}