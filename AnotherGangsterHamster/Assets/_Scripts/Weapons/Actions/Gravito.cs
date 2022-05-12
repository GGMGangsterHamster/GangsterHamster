using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Checkpoint;
using Matters.Gravity;

namespace Weapons.Actions
{
    public class Gravito : WeaponAction
    {
        public float gravityChangeTime;
        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle; 
        private CheckpointManager _checkpoint;
        private RaycastHit _aTypeHit;

        private CheckpointManager Checkpoint
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

        private float _currentGravityChangeTime = 0f;  
        private bool isChangedGravity = false;
        private bool isReseting = false; 

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Gravito;
        }

        public override void FireWeapon()
        {
            if(_currentGravitoStatus != GravitoStatus.Use &&
               _currentGravitoStatus != GravitoStatus.Stickly &&
               _currentGravitoStatus != GravitoStatus.ChangeGravity &&
               !isReseting)
            {
                if(Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit) && hit.transform.CompareTag("ATYPEOBJECT"))
                {
                    _fireDir = MainCameraTransform.forward;
                    _currentGravitoStatus = GravitoStatus.Stickly;
                    transform.position = hit.point - (_fireDir * transform.localScale.y);
                    transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0);

                    _aTypeHit = hit;
                }
            }
        }

        public override void UseWeapon()
        {
            if(_currentGravitoStatus == GravitoStatus.Stickly && !isChangedGravity)
            {
                //if (_aTypeHit.normal == Vector3.up) return;

                _currentGravitoStatus = GravitoStatus.ChangeGravity;
                _currentGravityChangeTime = 0f;
                isChangedGravity = true;

                Checkpoint.SetStartCheckpoint(PlayerBaseTransform.forward);
                Checkpoint.SetEndCheckpoint(_aTypeHit.normal);

                GravityManager.ChangeGlobalGravityDirection(-_aTypeHit.normal);
            }
        }

        public override void ResetWeapon()
        {
            if (isReseting)
                return;
            else if (!isChangedGravity)
            {
                _currentGravitoStatus = GravitoStatus.Idle;
                transform.rotation = Quaternion.identity;
                return;
            }

            _currentGravitoStatus = GravitoStatus.Reset;
            _currentGravityChangeTime = 0f;
            isChangedGravity = false;
            isReseting = true;

            Checkpoint.startCheckpoint.rotation = PlayerBaseTransform.rotation;
            Checkpoint.endCheckpoint.rotation = Quaternion.Euler(new Vector3(0, PlayerBaseTransform.rotation.eulerAngles.y, 0));

            GravityManager.ChangeGlobalGravityDirection(Vector3.down);
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
                    transform.position = HandPosition;
                    break;
                case GravitoStatus.ChangeGravity:
                    _currentGravityChangeTime += Time.deltaTime / gravityChangeTime;

                    if (_currentGravityChangeTime >= 1f)
                    {
                        PlayerBaseTransform.rotation = Checkpoint.endCheckpoint.rotation;
                        _currentGravitoStatus = GravitoStatus.Stickly;
                        Debug.Log(_aTypeHit.normal + " : " + PlayerBaseTransform.rotation.eulerAngles);
                    }
                    else
                    {
                        PlayerBaseTransform.rotation = Quaternion.Lerp(
                            Checkpoint.startCheckpoint.rotation,
                            Checkpoint.endCheckpoint.rotation,
                            _currentGravityChangeTime);
                    }
                    break;
                case GravitoStatus.Reset: 
                    _currentGravityChangeTime += Time.deltaTime / gravityChangeTime;

                    if(_currentGravityChangeTime >= 1f)
                    {
                        PlayerBaseTransform.rotation = Checkpoint.endCheckpoint.rotation;
                        _currentGravitoStatus = GravitoStatus.Idle;
                        transform.rotation = Quaternion.identity;
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
    }
}