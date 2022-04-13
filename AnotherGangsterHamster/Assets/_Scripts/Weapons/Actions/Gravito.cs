using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class Gravito : WeaponAction
    {
        public float gravityChangeSpeed;

        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle;

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
                if (_myRigid.constraints == RigidbodyConstraints.FreezePosition)
                    _myRigid.constraints = RigidbodyConstraints.None;
                
                _fireDir = MainCameraTransform.forward;
                transform.position = FirePosition;
                _currentGravitoStatus = GravitoStatus.Fire;
            }
        }

        public override void UseWeapon()
        {

        }

        public override void ResetWeapon()
        {
            transform.position = HandPosition;
            _currentGravitoStatus = GravitoStatus.Idle;

            // �߷� �ʱ�ȭ
        }

        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }

        public void ATypeObjectCollisionEnterEvent(GameObject obj)
        {
            if(_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
        }

        public void BTypeObjectCollisionEnterEvent(GameObject obj)
        {
            if (!obj.TryGetComponent(out BoxCollider boxCol))
            {
                // ���⼭ �̰����� ������ �ȵȴٴ� ���� �������� ��
                return;
            }

            if (_currentGravitoStatus == GravitoStatus.Fire)
            {
                Stop();
                _currentGravitoStatus = GravitoStatus.Stickly;
            }
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
                    _myRigid.velocity = _fireDir * fireSpeed;
                    break;
                case GravitoStatus.Stickly:
                    // �� ���¿� �ɷ� ���� ���� ���⿡ ���� �߷��� ��ȯ�Ǿ�� ��.
                    // ���� ���� ����.. ����� �κ�
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