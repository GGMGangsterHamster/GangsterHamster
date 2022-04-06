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
        private Dictionary<WeaponEnum, WeaponAction> _weaponActions;

        private WeaponEnum _curWeapon = WeaponEnum.None;

        private void Start()
        {
            _weaponActions = new Dictionary<WeaponEnum, WeaponAction>();
            _weaponActions.Clear();

            // �ڽ� ������Ʈ�� ������� ��ġ�Ǿ� �ִٴ� �����Ͽ� ������� �ڵ�
            WeaponAction[] childWeaponActions = transform.GetComponentsInChildren<WeaponAction>();

            foreach (WeaponAction weaponAction in childWeaponActions)
            {
                _weaponActions.Add(weaponAction._weaponEnum, weaponAction);
            }
        }

        public void FireCurrentWeapon()
        {
            if(_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].FireWeapon();
            }
        }

        public void UseCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].UseWeapon();
            }
        }

        public void ResetCurrentWeapon()
        {
            if (_curWeapon != WeaponEnum.None)
            {
                _weaponActions[_curWeapon].ResetWeapon();
            }
        }

        public void ChangeCurrentWeapon(WeaponEnum weaponEnum)
        {
            if(weaponEnum == WeaponEnum.None)
            {
                // �տ� ��� ���������� �����
            }
            else
            {
                if(_weaponActions[weaponEnum].possibleUse)
                {
                    _curWeapon = weaponEnum;
                }
            }
        }

        // InputHandler������ Ű�� �Է����ϰ� ȣ�⸸ ��
        // ���⼭�� �� ȣ�� �Ȱſ��� �� �ؼ� �������ָ� ��
    }
}
