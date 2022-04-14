using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Gravito : WeaponAction
    {
        public float gravityChangeTime;

        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle;

        private CollisionInteractableObject _colInteractableObj;

        private Vector3 colNormalVec
        {
            get
            {
                if(_colInteractableObj == null)
                {
                    _colInteractableObj = GetComponent<CollisionInteractableObject>();
                }

                return _colInteractableObj.objCollision.contacts[0].normal;
            }
        }

        private float _currentGravityChangeTime = 0f;

        private bool isChangedGravity = false;

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
               _currentGravitoStatus != GravitoStatus.ChangeGravity)
            {
                if (_myRigid.constraints != RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.None;
                
                _fireDir = MainCameraTransform.forward;
                transform.position = FirePosition;
                _currentGravitoStatus = GravitoStatus.Fire;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }

        public override void UseWeapon()
        {
            if(_currentGravitoStatus == GravitoStatus.Stickly && !isChangedGravity)
            {
                _currentGravitoStatus = GravitoStatus.ChangeGravity;
                _currentGravityChangeTime = 0f;
                isChangedGravity = true;

                Debug.Log(Quaternion.LookRotation(colNormalVec));

                // �� ������ �������� �߷��� ��ȯ�ǰ�,
                // ī�޶��� ���⵵ �����ϰ� ��ȯ�Ǿ�� ��
            }
        }

        public override void ResetWeapon()
        {
            _currentGravitoStatus = GravitoStatus.Idle;
            isChangedGravity = false;
            // �߷� �ʱ�ȭ
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

                Debug.Log(colNormalVec);

                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        public void BTypeObjectCollisionEnterEvent(GameObject obj)
        {
            if (_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                
                Debug.Log(colNormalVec);

                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        private void Update()
        {
            switch(_currentGravitoStatus)
            {
                case GravitoStatus.Idle:
                    if (!_myCollider.isTrigger) _myCollider.isTrigger = true;
                    if (_myRigid.useGravity) _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                    transform.position = HandPosition;
                    break;
                case GravitoStatus.Fire:
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;
                case GravitoStatus.Stickly:



                    // �� ���¿� �ɷ� ���� ���� ���⿡ ���� �߷��� ��ȯ�Ǿ�� ��.
                    // ���� ���� ����.. ����� �κ�
                    break;
                case GravitoStatus.ChangeGravity:
                    _currentGravityChangeTime += Time.deltaTime / gravityChangeTime;

                    if (_currentGravityChangeTime >= 1f)
                    {
                        PlayerBaseTransform.rotation = Quaternion.LookRotation(colNormalVec);
                        _currentGravitoStatus = GravitoStatus.Stickly;
                    }
                    else
                    {
                        PlayerBaseTransform.rotation = Quaternion.Lerp(
                            PlayerBaseTransform.rotation,
                            Quaternion.LookRotation(colNormalVec),
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


        }
    }
}