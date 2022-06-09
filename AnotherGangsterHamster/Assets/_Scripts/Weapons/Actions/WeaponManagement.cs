using Matters.Velocity;
using Objects.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public enum WeaponEnum
    {
        None,
        Lumo,
        Grand,
        Gravito,
    }
    public class WeaponManagement : MonoBehaviour
    {
        public WeaponEnum startHandleWeapon;

        private Transform _playerBaseTransform;

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

        private Transform grandCharge;
        private Transform lumoCube;
        private Dictionary<WeaponEnum, WeaponAction> _weaponActions;

        private WeaponEnum _curWeapon = WeaponEnum.None;

        private void Awake()
        {
            _weaponActions = new Dictionary<WeaponEnum, WeaponAction>();
            _weaponActions.Clear();

            _curWeapon = startHandleWeapon;
        }

        private void Start()
        {
            // 자식 오브젝트가 1번무기, 2번무기, 3번무기 순서대로 배치되어 있다는 전제하에 만들어진 코드
            WeaponAction[] childWeaponActions = transform.GetComponentsInChildren<WeaponAction>();

            foreach (WeaponAction weaponAction in childWeaponActions)
            {
                _weaponActions.Add(weaponAction._weaponEnum, weaponAction);

                weaponAction.gameObject.SetActive(startHandleWeapon == weaponAction._weaponEnum);
            }

            grandCharge = GameObject.Find("GrandCharge").transform;
            lumoCube = _weaponActions[WeaponEnum.Lumo].transform.GetChild(0);
            grandCharge.gameObject.SetActive(startHandleWeapon == WeaponEnum.Grand);
        }

        // 좌클릭 시 발동되는 함수
        public void FireCurrentWeapon()
        {
            if(!InteractionManager.Instance.GetGrep())
            {
                if (_curWeapon != WeaponEnum.None && _weaponActions[_curWeapon].possibleUse)
                {
                    _weaponActions[_curWeapon].FireWeapon();
                }
            }
        }

        // 우클릭 시 발동되는 함수
        public void UseCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None && _weaponActions[_curWeapon].possibleUse)
            {
                _weaponActions[_curWeapon].UseWeapon();
            }
        }

        // R키 누를시 발동되는 함수
        public void ResetCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None && _weaponActions[_curWeapon].possibleUse)
            {
                _weaponActions[_curWeapon].ResetWeapon();

                if (_weaponActions[WeaponEnum.Lumo].gameObject.activeSelf)
                    PlayerBaseTransform.GetComponent<FollowGroundPos>().Deactive(lumoCube.gameObject);

                if(_curWeapon == WeaponEnum.Grand)
                {
                    // 그랜드에 무기가 붙어있는데 
                    // 그랜드를 리셋 시키면 같이 리셋 됨
                    if (_weaponActions[WeaponEnum.Lumo].GetComponent<Lumo>().SticklyTrm() == _weaponActions[WeaponEnum.Grand].transform)
                    {
                        _weaponActions[WeaponEnum.Lumo].gameObject.SetActive(false);
                        _weaponActions[WeaponEnum.Lumo].ResetWeapon();
                    }
                    if(_weaponActions[WeaponEnum.Gravito].GetComponent<Gravito>().SticklyTrm() == _weaponActions[WeaponEnum.Grand].transform)
                    {
                        _weaponActions[WeaponEnum.Gravito].ResetWeapon();
                    }
                }

                if (InteractionManager.Instance.GetGrep())
                {
                    PlayerBaseTransform.GetComponent<FollowGroundPos>().Deactive(_weaponActions[_curWeapon].gameObject);
                    _weaponActions[_curWeapon].gameObject.SetActive(false);
                }
            }
        }

        // 1,2,3번 같이 숫자 누르면 발동되는 함수
        public void ChangeCurrentWeapon(WeaponEnum weaponEnum)
        {
            bool isChanged = false;
            
            foreach(WeaponAction weaponAction in _weaponActions.Values)
            {
                if (weaponAction.SetActiveWeaponObj(weaponEnum))
                {
                    _curWeapon = weaponEnum;
                    grandCharge.gameObject.SetActive(_curWeapon == WeaponEnum.Grand && _weaponActions[_curWeapon].possibleUse);
                    isChanged = true;
                }
            }

            if (!isChanged)
                _curWeapon = WeaponEnum.None;
        }

        public WeaponEnum GetCurrentWeapon()
        {
            return _curWeapon;
        }

        public WeaponAction GetCurrentWeaponAction()
        {
            if (_curWeapon == WeaponEnum.None || !_weaponActions.ContainsKey(_curWeapon) || !_weaponActions[_curWeapon].possibleUse) return null;

            return _weaponActions[_curWeapon];
        }
    }
}
