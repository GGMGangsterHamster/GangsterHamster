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
                weaponAction.gameObject.SetActive(false);
            }
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
                if(weaponAction.SetActiveWeaponObj(weaponEnum))
                {
                    _curWeapon = weaponEnum;
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
        // InputHandler������ Ű�� �Է����ϰ� ȣ�⸸ ��
        // ���⼭�� �� ȣ�� �Ȱſ��� �� �ؼ� �������ָ� ��
    }
}
