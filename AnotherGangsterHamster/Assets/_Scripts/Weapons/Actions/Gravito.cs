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

        private CollisionInteractableObject _colInteractableObj;
        private CheckpointManager _checkpoint;

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

        private Vector3 colNormalVec
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
            if(_currentGravitoStatus != GravitoStatus.Fire && 
               _currentGravitoStatus != GravitoStatus.Use &&
               _currentGravitoStatus != GravitoStatus.Stickly &&
               _currentGravitoStatus != GravitoStatus.ChangeGravity &&
               !isReseting)
            {
                if (_myRigid.constraints != RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.None;
                
                _fireDir = MainCameraTransform.forward;
                transform.position = FirePosition;
                _myRigid.angularVelocity = Vector3.zero;
                _currentGravitoStatus = GravitoStatus.Fire;

                transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0);

                _myRigid.velocity = _fireDir * fireSpeed;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }

        public override void UseWeapon()
        {
            if(_currentGravitoStatus == GravitoStatus.Stickly && !isChangedGravity)
            {
                if (colNormalVec == Vector3.up) return;

                _currentGravitoStatus = GravitoStatus.ChangeGravity;
                _currentGravityChangeTime = 0f;
                isChangedGravity = true;

                Checkpoint.SetStartCheckpoint(PlayerBaseTransform.forward);
                Checkpoint.SetEndCheckpoint(colNormalVec);


                GravityManager.ChangeGlobalGravityDirection(-colNormalVec);
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
            Checkpoint.endCheckpoint.rotation = Quaternion.Euler(new Vector3(0, PlayerBaseTransform.rotation.y, 0));

            GravityManager.ChangeGlobalGravityDirection(Vector3.down);
        }

        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }

        public void ATypeObjectCollisionEnterEvent(GameObject obj)
        {            
            if (_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }
        public void BTypeObjectCollisionEnterEvent(GameObject obj)
        {
            if (_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        private void Update()
        {
            Debug.Log(PlayerBaseTransform.rotation.eulerAngles);
            switch(_currentGravitoStatus)
            {
                case GravitoStatus.Idle:
                    if (!_myCollider.isTrigger) _myCollider.isTrigger = true;
                    if (_myRigid.useGravity) _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                    transform.position = HandPosition;
                    break;
                case GravitoStatus.Fire: 

                    break;
                case GravitoStatus.Stickly:
                    break;
                case GravitoStatus.ChangeGravity:
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

        private void Stop()
        {
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0);
        }
    }
}