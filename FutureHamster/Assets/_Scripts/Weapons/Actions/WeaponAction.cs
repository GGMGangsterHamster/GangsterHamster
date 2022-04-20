using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
using Characters.Player;

namespace Weapons.Actions
{
    #region WeaponEnum
    // �̳ʽÿ��� ���� �� �ִ� ���µ�
    public enum InercioStatus
    {
        Idle,
        Fire,
        Use,
        Stickly,
        LosePower,
    }

    // �׷��尡 ���� �� �ִ� ���µ�
    public enum GrandStatus
    {
        Idle,
        Fire,
        Use,
        Resize,
        LosePower,
    }

    public enum GravitoStatus
    {
        Idle,
        Fire,
        Use,
        Stickly,
        ChangeGravity,
        Reset,
    }
    #endregion

    // �̳ʽÿ�, �׷���, �׷����� 3������ ������� �����ϱ� ���� �θ� Ŭ�����̸�
    // ����� ��ɵ��� �Լ��� ���� �س��⵵ �Ͽ���.
    [RequireComponent(typeof(CollisionInteractableObject))]
    [RequireComponent(typeof(TriggerInteractableObject))]
    public class WeaponAction : MonoBehaviour
    {
        [HideInInspector] public WeaponEnum _weaponEnum; // ��ӹ��� ������ ����

        #region Propertys
        [HideInInspector] public string Path = "SettingValue/HandMode.json";    // �ַ� ����ϴ� ���� �����ΰ��� ������ �ִ� ���� ��ġ
        public bool possibleUse = false;                                        // ���⸦ ����� �����Ѱ�
        public float fireSpeed;                                                 // ���Ⱑ �����̴� �ӵ��� ����
        public int weaponDamage;                                                // ���� ������

        protected Player _player;
        protected Transform _mainCameraTransform;
        protected Transform _playerBaseTransform;
        protected Transform _playerTransform;

        private HandModeVO _handModeVO;

        protected Player Player
        {
            get
            {
                if(_player == null)
                {
                    _player = PlayerBaseTransform.GetComponent<Player>();
                }

                return _player;
            }
        }
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

        protected Transform PlayerTrasnform
        {
            get
            {
                if (_playerTransform == null)
                {
                    _playerTransform = GameObject.FindGameObjectWithTag("PLAYER").transform;
                }

                return _playerTransform;
            }
        }

        private bool IsRightHand
        {
            get
            {
                if(_handModeVO == null)
                {
                    _handModeVO = Utils.JsonToVO<HandModeVO>(Path);
                }

                return _handModeVO.isRightHand;
            }
        }

        protected Vector3 HandPosition => PlayerBaseTransform.position
                              + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                              + MainCameraTransform.forward
                              + PlayerBaseTransform.right * (IsRightHand ? 1 : -1);

        protected Vector3 FirePosition => MainCameraTransform.position + MainCameraTransform.forward / 2;

        protected Collider _myCollider;
        protected Rigidbody _myRigid;

        protected Vector3 _fireDir;                                             // ���⸦ �߻��� �� ���ư��� ����

        #endregion

        // �⺻���� �Լ���

        virtual protected void Awake()
        {
            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// ��Ŭ������ ���� �߻�
        /// </summary>
        public virtual void FireWeapon()
        {

        }

        /// <summary>
        /// ��Ŭ������ �ɷ� �ߵ�
        /// </summary>
        public virtual void UseWeapon()
        {

        }

        /// <summary>
        /// R�� ���� ȸ��
        /// </summary>
        public virtual void ResetWeapon()
        {

        }

        /// <summary>
        /// �׳� ���� �÷��̾ �ش��ϴ� ������Ʈ�� ��� �ֳ��� �˻��ϴ� �Լ�
        /// </summary>
        /// <returns></returns>
        public virtual bool IsHandleWeapon()
        {
            return false;
        }

        /// <summary>
        /// ���� ���ڰ��� ���� SetActive True, false ���ִ� �Լ�
        /// </summary>
        public bool SetActiveWeaponObj(WeaponEnum wenum)
        {
            if (!possibleUse) return false;

            gameObject.SetActive(wenum == _weaponEnum || !IsHandleWeapon());

            return gameObject.activeSelf;
        }


    }
}