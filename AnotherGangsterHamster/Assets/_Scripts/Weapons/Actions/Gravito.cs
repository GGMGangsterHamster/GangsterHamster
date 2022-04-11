using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Gravito : WeaponAction
    {
        public string Path = "SettingValue/HandMode.json";
        public float fireSpeed;
        public float gravityChangeSpeed;
        private enum GravitoStatus
        {
            Idle,
            Fire,
            Use,
            ChangeGravity,
        }

        #region Propertys

        private Transform _mainCameraTransform;
        private Transform _playerBaseTransform;
        private Transform _playerTrasnform;

        private Transform MainCameraTransform
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

        private Transform PlayerBaseTransform
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

        private Vector3 HandPosition => PlayerBaseTransform.position
                                      + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                                      + MainCameraTransform.forward
                                      + PlayerBaseTransform.right * (Utils.JsonToVO<HandModeVO>(Path).isRightHand ? 1 : -1);

        #endregion

        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle;
        
        private Collider _myCollider;
        private Rigidbody _myRigid;
        
        private Vector3 _fireDir;

        private void Awake()
        {
            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();

            _weaponEnum = WeaponEnum.Gravito;
        }

        public override void FireWeapon()
        {
            if(_currentGravitoStatus != GravitoStatus.Fire && _currentGravitoStatus != GravitoStatus.Use)
            {
                _fireDir = MainCameraTransform.forward;
                _currentGravitoStatus = GravitoStatus.Fire;
            }
        }

        public override void UseWeapon()
        {

        }

        public override void ResetWeapon()
        {
            _currentGravitoStatus = GravitoStatus.Idle;
        }

        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }

        private void Update()
        {
            switch(_currentGravitoStatus)
            {
                case GravitoStatus.Idle:
                    if (!_myCollider.isTrigger)
                        _myCollider.isTrigger = true;
                    if (_myRigid.useGravity)
                        _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None)
                        _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                    transform.position = HandPosition;
                    break;
                case GravitoStatus.Fire:
                    transform.position += _fireDir * Time.deltaTime * fireSpeed;
                    break;
            }
        }
    }
}