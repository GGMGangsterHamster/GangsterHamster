using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Inercio : WeaponAction
    {
        public string Path = "SettingValue/HandMode.json";
        public float FireSpeed;

        // 무기가 가질 수 있는 상황을 모아놓음
        private enum InercioStatus
        {
            Idle,
            Fire,
            Use,
            Stickly,
        }

        private Transform _mainCameraTransform;
        private Transform _playerBaseTransform;
        private Transform _playerTrasnform;

        #region Propertys
        private Transform MainCameraTransform
        {
            get
            {
                if(_mainCameraTransform == null)
                {
                    _mainCameraTransform = Camera.main.transform;
                }

                return _mainCameraTransform;
            }
        }
        private Transform PlayerBaseTransform
        {
            get
            {
                if(_playerBaseTransform == null)
                {
                    _playerBaseTransform = GameObject.FindGameObjectWithTag("PLAYER_BASE").transform;
                }

                return _playerBaseTransform;
            }
        }
        private Transform PlayerTrasnform
        {
            get
            {
                if (_playerTrasnform == null)
                {
                    _playerTrasnform = GameObject.FindGameObjectWithTag("PLAYER").transform;
                }

                return _playerTrasnform;
            }
        }

        // 플레이어 위치에서
        // 플레이어 높이에 맞추고
        // 앞으로 조금 이동하고
        // 왼손 오른손에 따라 이동한다
        private Vector3 HandPosition => PlayerBaseTransform.position
                                      + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                                      + MainCameraTransform.forward 
                                      + PlayerBaseTransform.right * (Utils.JsonToVO<HandModeVO>(Path).isRightHand ? 1 : -1);
        private bool isCanFire
        {
            get
            {
                // 잠시 임시로 넣은거 이후 바뀔 코드이빈다
                return FindObjectOfType<WeaponManagement>().transform == transform.parent;
            }
        }
        #endregion

        private InercioStatus _currentStatus = InercioStatus.Idle;

        private Collision _sticklyObjectCollision;
        private Vector3 _fireDir;

        private void Awake()
        {
            _weaponEnum = WeaponEnum.Inercio;
        }

        public override void FireWeapon()
        {
            if (isCanFire && _currentStatus != InercioStatus.Fire)
            {
                _fireDir = MainCameraTransform.forward;

                _currentStatus = InercioStatus.Fire;
            }
        }

        public override void UseWeapon()
        {
            _currentStatus = InercioStatus.Use;
        }

        public override void ResetWeapon()
        {
            _currentStatus = InercioStatus.Idle;

            _sticklyObjectCollision = null;
        }

        #region CollisionEvents
        public void PlayerCollisionEvent(Collision col)
        {
            ResetWeapon();
        }
        public void ATypeObjectCollisionEnterEvent(Collision col)
        {
            if (_sticklyObjectCollision != null && _sticklyObjectCollision.gameObject == obj.gameObject)
            {
                return;
            }
            
            _currentStatus = InercioStatus.Stickly;
            _sticklyObjectCollision = col;
        }
        public void ATypeObjectCollisionExitEvent(Collision col)
        {

        }
        public void BTypeObjectCollisionEnterEvent(Collision col)
        {
            
        }
        #endregion

        private void Update()
        {
            switch(_currentStatus)
            {
                case InercioStatus.Idle:
                    transform.position = HandPosition;
                    break;
                case InercioStatus.Fire:
                    transform.position += _fireDir * Time.deltaTime * FireSpeed;
                    break;
                case InercioStatus.Use:
                    transform.position += (MainCameraTransform.position - transform.position).normalized * Time.deltaTime * FireSpeed;


                    break;
                case InercioStatus.Stickly:
                    Debug.Log("Stickly 상태");
                    break;
            }
        }
    }
}