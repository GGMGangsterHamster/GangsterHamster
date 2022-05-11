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

            // �ڽ� ������Ʈ�� 1������, 2������, 3������ ������� ��ġ�Ǿ� �ִٴ� �����Ͽ� ������� �ڵ�
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

        // ��Ŭ�� �� �ߵ��Ǵ� �Լ�
        public void FireCurrentWeapon()
        {
            if(_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].FireWeapon();
            }
        }

        // ��Ŭ�� �� �ߵ��Ǵ� �Լ�
        public void UseCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].UseWeapon();
            }
        }

        // RŰ ������ �ߵ��Ǵ� �Լ�
        public void ResetCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].ResetWeapon();
            }
        }

        // 1,2,3�� ���� ���� ������ �ߵ��Ǵ� �Լ�
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
