using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
using Characters.Player;
using Objects.Interaction;

namespace Weapons.Actions
{
    #region WeaponEnum
    // 이너시오가 가질 수 있는 상태들
    public enum InercioStatus
    {
        Idle,
        Fire,
        Use,
        LosePower,
        Selected,
        OneSelected,
        TwoSelected,
    }

    public enum StopStatus
    {
        Idle,
        Use,
        Stickly,
        Stay
    }

    public enum LumoStatus
    {
        Idle,
        Use,
        Stickly
    }

    public enum ConnectionStatus
    {
        Idle,
        OneStickly,
        TwoStickly,
        Use
    }

    // 그랜드가 가질 수 있는 상태들
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
        Use,
        Stickly,
        ChangeGravity,
        Reset,
    }
    #endregion

    // 이너시오, 그랜드, 그래비토 3가지의 무기들을 관리하기 위한 부모 클래스이며
    // 공통된 기능들을 함수로 정리 해놓기도 하였다.
    public class WeaponAction : MonoBehaviour
    {
        [HideInInspector] public WeaponEnum _weaponEnum; // 상속받은 무기의 종류

        #region Propertys
        [HideInInspector] public string Path = "SettingValue/HandMode.json";    // 주로 사용하는 손이 무엇인가의 정보가 있는 곳의 위치
        public bool possibleUse = false;                                        // 무기를 사용이 가능한가
        public float fireSpeed;                                                 // 무기가 움직이는 속도를 말함
        public int weaponDamage;                                                // 무기 데미지

        private Player _player;
        private Transform _mainCameraTransform;
        private Transform _playerBaseTransform;
        private Transform _playerTransform;
        private Transform _weaponObjectParentTransform;
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

        protected Transform WeaponObjectParentTransform
        {
            get
            {
                if(_weaponObjectParentTransform == null)
                {
                    _weaponObjectParentTransform = GameObject.Find("WeaponObjects").transform;
                }

                return _weaponObjectParentTransform;
            }
        }

        protected Vector3 HandPosition => PlayerBaseTransform.position
                              + PlayerBaseTransform.up * (PlayerTrasnform.localScale.y - 0.5f)
                              + MainCameraTransform.forward
                              + PlayerBaseTransform.right;

        protected Vector3 FirePosition => MainCameraTransform.position
                                        + MainCameraTransform.forward * (PlayerStatus.IsCrouching ? 0.4f : 1f)
                                        ;

        protected Collider _myCollider;
        protected Rigidbody _myRigid;

        protected Vector3 _fireDir;                                             // 무기를 발사할 때 나아가는 방향

        #endregion

        // 기본적인 함수들

        virtual protected void Awake()
        {
            _myCollider = GetComponent<Collider>();
            _myRigid = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// 좌클릭으로 무기 발사
        /// </summary>
        public virtual void FireWeapon()
        {

        }

        /// <summary>
        /// 우클릭으로 능력 발동
        /// </summary>
        public virtual void UseWeapon()
        {

        }

        /// <summary>
        /// R로 무기 회수
        /// </summary>
        public virtual void ResetWeapon()
        {

        }

        public void SetHandPosition()
        {
            transform.position = HandPosition;
        }

        /// <summary>
        /// 그냥 지금 플레이어가 해당하는 오브젝트를 들고 있나를 검사하는 함수
        /// </summary>
        /// <returns></returns>
        public virtual bool IsHandleWeapon()
        {
            return false;
        }

        public virtual Transform SticklyTrm()
        {
            return null;
        }

        /// <summary>
        /// 들어온 인자값에 따라 SetActive True, false 해주는 함수
        /// </summary>
        public void SetActiveWeaponObj(WeaponEnum wenum)
        {
            gameObject.SetActive((wenum == _weaponEnum && !InteractionManager.Instance.GetGrep()) || !IsHandleWeapon());
        }

        public void ResetPosiiton()
        {
            transform.position = HandPosition;
        }
    }
}