using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public enum WeaponEnum
    {
        None,
        Inercio,
        Grand,
        Gravito,
    }
    public class WeaponManagement : MonoBehaviour
    {
        private Transform grandCharge;
        private Transform stopCharge;

        private Dictionary<WeaponEnum, WeaponAction> _weaponActions;

        private WeaponEnum _curWeapon = WeaponEnum.None;

        private void Start()
        {
            _weaponActions = new Dictionary<WeaponEnum, WeaponAction>();
            _weaponActions.Clear();

            // 자식 오브젝트가 1번무기, 2번무기, 3번무기 순서대로 배치되어 있다는 전제하에 만들어진 코드
            WeaponAction[] childWeaponActions = transform.GetComponentsInChildren<WeaponAction>();

            foreach (WeaponAction weaponAction in childWeaponActions)
            {
                _weaponActions.Add(weaponAction._weaponEnum, weaponAction);
                weaponAction.gameObject.SetActive(false);
            }

            grandCharge = GameObject.Find("GrandCharge").transform;
            grandCharge.gameObject.SetActive(false);

            stopCharge = GameObject.Find("StopCharge").transform;
            stopCharge.gameObject.SetActive(false);
        }

        // 좌클릭 시 발동되는 함수
        public void FireCurrentWeapon()
        {
            if(_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].FireWeapon();
            }
        }

        // 우클릭 시 발동되는 함수
        public void UseCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].UseWeapon();
            }
        }

        // R키 누를시 발동되는 함수
        public void ResetCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].ResetWeapon();
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
                    stopCharge.gameObject.SetActive(_curWeapon == WeaponEnum.Inercio);
                    grandCharge.gameObject.SetActive(_curWeapon == WeaponEnum.Grand);
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
    }
}
