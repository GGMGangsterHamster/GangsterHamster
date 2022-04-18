using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Checkpoint;
using Physics.Gravity;

namespace Weapons.Actions
{
    public class Gravito : WeaponAction
    {
        public float gravityChangeTime; // 중력 변환 할때 걸리는 시간

        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle; // 현재 무기의 상태임

        private CollisionInteractableObject _colInteractableObj; // 무기와 충돌한 오브젝트
        private CheckpointManager _checkpoint;

        private CheckpointManager Checkpoint // 중력변환의 처음과 끝을 가져오는 프로퍼티로 선형 변환 할려고 사용함
        {
            get
            {
                if(_checkpoint == null)
                {
                    _checkpoint = FindObjectOfType<CheckpointManager>();
                }

                return _checkpoint;
            }
        }

        private Vector3 colNormalVec // 부딪힌 오브젝트의 법선.
        {
            get
            {
                if(_colInteractableObj == null)
                {
                    _colInteractableObj = GetComponent<CollisionInteractableObject>();
                }

                return _colInteractableObj.colNormalVec;
            }
        }

        private float _currentGravityChangeTime = 0f; // Lerp 하려고 만든 변수 
        private bool isChangedGravity = false;
        private bool isReseting = false; // 지금 Reset하는 중인가 아닌가

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Gravito; // 이 무기는 Gravito 에요
        }

        /// <summary>
        /// 현재 상태가 Fire, Use, Stickly, ChangeGravity가 아니고 지금 리셋하는 중이 아니라면 실행함
        /// 
        /// 현재 플레이어가 바라보는 방향으로 무기를 날림
        /// (Update의 Fire 상태에 날려보내는 코드 있음)
        /// </summary>
        public override void FireWeapon()
        {
            if(_currentGravitoStatus != GravitoStatus.Fire && 
               _currentGravitoStatus != GravitoStatus.Use &&
               _currentGravitoStatus != GravitoStatus.Stickly &&
               _currentGravitoStatus != GravitoStatus.ChangeGravity &&
               !isReseting)
            {
                if (_myRigid.constraints != RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.None;
                
                _fireDir = MainCameraTransform.forward;
                transform.position = FirePosition;
                _currentGravitoStatus = GravitoStatus.Fire;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }

        /// <summary>
        /// 현재 상태가 Stickly이며 지금 중력을 변환중이 아니라면 사용가능
        /// 
        /// 중력변환을 하고 플레이어를 중력에 맞춰서 화면을 돌린다.
        /// </summary>
        public override void UseWeapon()
        {
            if(_currentGravitoStatus == GravitoStatus.Stickly && !isChangedGravity)
            {
                _currentGravitoStatus = GravitoStatus.ChangeGravity;
                _currentGravityChangeTime = 0f;
                isChangedGravity = true;

                Checkpoint.SetStartCheckpoint(PlayerBaseTransform.forward);
                Checkpoint.SetEndCheckpoint(colNormalVec);

                GravityManager.ChangeGlobalGravityDirection(-colNormalVec);
            }
        }

        /// <summary>
        /// 지금 리셋증이라면 실행 안함
        /// 또는 지금 중력을 바꾸지 않은 상태라면 무기를 회수한다.
        /// 
        /// 이외의 상황은 중력이 바꿔져 있는 상태인 경우에만 실행이 되는데
        /// 중력을 원래 상태로 만들고 그 중력대로 화면을 회전 시킨다.
        /// </summary>
        public override void ResetWeapon()
        {
            if(isReseting)
                return;
            else if (!isChangedGravity)
            {
                _currentGravitoStatus = GravitoStatus.Idle;
                return;
            }

            _currentGravitoStatus = GravitoStatus.Reset;
            _currentGravityChangeTime = 0f;
            isChangedGravity = false;
            isReseting = true;

            Checkpoint.startCheckpoint.rotation = PlayerBaseTransform.rotation;
            Checkpoint.endCheckpoint.rotation = Quaternion.Euler(new Vector3(0, PlayerBaseTransform.rotation.y, 0));

            GravityManager.ChangeGlobalGravityDirection(Vector3.down);
        }

        /// <summary>
        /// 지금 손에 이 무기가 있는가를 판별함
        /// </summary>
        /// <returns></returns>
        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }

        /// <summary>
        /// ATypeObj와 충돌하면 멈추며 Stickly 상태로 변환.
        /// </summary>
        /// <param name="obj"></param>
        public void ATypeObjectCollisionEnterEvent(GameObject obj)
        {            
            if (_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        /// <summary>
        /// 위와 똑같음
        /// </summary>
        /// <param name="obj"></param>
        public void BTypeObjectCollisionEnterEvent(GameObject obj)
        {
            if (_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        /// <summary>
        /// 지금 무기의 현재 상태에 따라서 행동을 달리함
        /// Win32의 메시지 방식을 참조해서 조금 따라한 느낌이 있음..
        /// </summary>
        private void Update()
        {
            switch(_currentGravitoStatus)
            {
                case GravitoStatus.Idle: // HandPosition에 무기 고정
                    if (!_myCollider.isTrigger) _myCollider.isTrigger = true;
                    if (_myRigid.useGravity) _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                    transform.position = HandPosition;
                    break;
                case GravitoStatus.Fire: // _fireDir로 계속해서 날아가기
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;
                case GravitoStatus.Stickly:
                    // 이 상태에 능력 사용시 벽의 방향에 따라 중력이 변환되어야 함.
                    break;
                case GravitoStatus.ChangeGravity: // 화면을 Lerp를 통해 회전시킴
                    _currentGravityChangeTime += Time.deltaTime / gravityChangeTime;

                    if (_currentGravityChangeTime >= 1f)
                    {
                        PlayerBaseTransform.rotation = Checkpoint.endCheckpoint.rotation;
                        _currentGravitoStatus = GravitoStatus.Stickly;
                    }
                    else
                    {
                        PlayerBaseTransform.rotation = Quaternion.Lerp(
                            Checkpoint.startCheckpoint.rotation,
                            Checkpoint.endCheckpoint.rotation,
                            _currentGravityChangeTime);
                    }
                    break;
                case GravitoStatus.Reset: // 화면을 Lerp를 통해 회전시킴
                    _currentGravityChangeTime += Time.deltaTime / gravityChangeTime;

                    if(_currentGravityChangeTime >= 1f)
                    {
                        PlayerBaseTransform.rotation = Quaternion.Euler(new Vector3(0, PlayerBaseTransform.rotation.y, 0));
                        _currentGravitoStatus = GravitoStatus.Idle;
                        isReseting = false;
                    }
                    else
                    {
                        PlayerBaseTransform.rotation = Quaternion.Lerp(
                            Checkpoint.startCheckpoint.rotation,
                            Checkpoint.endCheckpoint.rotation,
                            _currentGravityChangeTime);
                    }
                    break;
            }
        }

        /// <summary>
        /// 무기를 못 움직이게 하기.
        /// </summary>
        private void Stop()
        {
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
        }
    }
}