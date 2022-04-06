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

            // 자식 오브젝트가 순서대로 배치되어 있다는 전제하에 만들어진 코드
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
                // 손에 든게 없어지도록 만들기
            }
            else
            {
                if(_weaponActions[weaponEnum].possibleUse)
                {
                    _curWeapon = weaponEnum;
                }
            }
        }

        // InputHandler에서는 키를 입력함하고 호출만 함
        // 여기서는 그 호출 된거에만 잘 해서 반응해주면 됨
    }
}
