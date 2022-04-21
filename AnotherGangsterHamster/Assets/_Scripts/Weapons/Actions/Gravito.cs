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
        public float gravityChangeTime; // �߷� ��ȯ �Ҷ� �ɸ��� �ð�

        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle; // ���� ������ ������

        private CollisionInteractableObject _colInteractableObj; // ����� �浹�� ������Ʈ
        private CheckpointManager _checkpoint;

        private CheckpointManager Checkpoint // �߷º�ȯ�� ó���� ���� �������� ������Ƽ�� ���� ��ȯ �ҷ��� �����
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

        private Vector3 colNormalVec // �ε��� ������Ʈ�� ����.
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

        private float _currentGravityChangeTime = 0f; // Lerp �Ϸ��� ���� ���� 
        private bool isChangedGravity = false;
        private bool isReseting = false; // ���� Reset�ϴ� ���ΰ� �ƴѰ�

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Gravito; // �� ����� Gravito ����
        }

        /// <summary>
        /// ���� ���°� Fire, Use, Stickly, ChangeGravity�� �ƴϰ� ���� �����ϴ� ���� �ƴ϶�� ������
        /// 
        /// ���� �÷��̾ �ٶ󺸴� �������� ���⸦ ����
        /// (Update�� Fire ���¿� ���������� �ڵ� ����)
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
                _myRigid.angularVelocity = Vector3.zero;
                _currentGravitoStatus = GravitoStatus.Fire;

                // ���⼭ �߻��ϴ� �������� �� ���Ⱑ �װ��� �ٶ󺸰� �ؾ� ��.
                transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0);

                _myRigid.velocity = _fireDir * fireSpeed;

                if (_myCollider.isTrigger)
                    _myCollider.isTrigger = false;
            }
        }

        /// <summary>
        /// ���� ���°� Stickly�̸� ���� �߷��� ��ȯ���� �ƴ϶�� ��밡��
        /// 
        /// ���� �ε��� ���� �ٴ��̶�� �н�
        /// 
        /// �߷º�ȯ�� �ϰ� �÷��̾ �߷¿� ���缭 ȭ���� ������.
        /// </summary>
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

        /// <summary>
        /// ���� �������̶�� ���� ����
        /// �Ǵ� ���� �߷��� �ٲ��� ���� ���¶�� ���⸦ ȸ���Ѵ�.
        /// 
        /// �̿��� ��Ȳ�� �߷��� �ٲ��� �ִ� ������ ��쿡�� ������ �Ǵµ�
        /// �߷��� ���� ���·� ����� �� �߷´�� ȭ���� ȸ�� ��Ų��.
        /// </summary>
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

        /// <summary>
        /// ���� �տ� �� ���Ⱑ �ִ°��� �Ǻ���
        /// </summary>
        /// <returns></returns>
        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }

        /// <summary>
        /// ATypeObj�� �浹�ϸ� ���߸� Stickly ���·� ��ȯ.
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
        /// ���� �Ȱ���
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
        /// ���� ������ ���� ���¿� ���� �ൿ�� �޸���
        /// Win32�� �޽��� ����� �����ؼ� ���� ������ ������ ����..
        /// </summary>
        private void Update()
        {
            switch(_currentGravitoStatus)
            {
                case GravitoStatus.Idle: // HandPosition�� ���� ����
                    if (!_myCollider.isTrigger) _myCollider.isTrigger = true;
                    if (_myRigid.useGravity) _myRigid.useGravity = false;
                    if (_myRigid.constraints == RigidbodyConstraints.None) _myRigid.constraints = RigidbodyConstraints.FreezePosition;

                    transform.position = HandPosition;
                    break;
                case GravitoStatus.Fire: // _fireDir�� ����ؼ� ���ư���

                    break;
                case GravitoStatus.Stickly:
                    // �� ���¿� �ɷ� ���� ���� ���⿡ ���� �߷��� ��ȯ�Ǿ�� ��.
                    break;
                case GravitoStatus.ChangeGravity: // ȭ���� Lerp�� ���� ȸ����Ŵ
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
                case GravitoStatus.Reset: // ȭ���� Lerp�� ���� ȸ����Ŵ
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

        /// <summary>
        /// ���⸦ �� �����̰� �ϱ�.
        /// </summary>
        private void Stop()
        {
            _myRigid.constraints = RigidbodyConstraints.FreezeAll;
            _myRigid.velocity = Vector3.zero;
            _myRigid.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0);
        }
    }
}