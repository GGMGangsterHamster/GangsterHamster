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
        private Transform grandCharge;

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
            // �ڽ� ������Ʈ�� 1������, 2������, 3������ ������� ��ġ�Ǿ� �ִٴ� �����Ͽ� ������� �ڵ�
            WeaponAction[] childWeaponActions = transform.GetComponentsInChildren<WeaponAction>();

            foreach (WeaponAction weaponAction in childWeaponActions)
            {
                _weaponActions.Add(weaponAction._weaponEnum, weaponAction);

                weaponAction.gameObject.SetActive(startHandleWeapon == weaponAction._weaponEnum);
            }

            grandCharge = GameObject.Find("GrandCharge").transform;
            grandCharge.gameObject.SetActive(startHandleWeapon == WeaponEnum.Grand);
        }

        // ��Ŭ�� �� �ߵ��Ǵ� �Լ�
        public void FireCurrentWeapon()
        {
            if(!InteractionManager.Instance.GetGrep())
            {
                if (_curWeapon != WeaponEnum.None)
                {
                    _weaponActions[_curWeapon].FireWeapon();
                }
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

                if(InteractionManager.Instance.GetGrep())
                {
                    _weaponActions[_curWeapon].gameObject.SetActive(false);
                }
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
                    grandCharge.gameObject.SetActive(_curWeapon == WeaponEnum.Grand);
                    isChanged = true;
                }
            }

            //if (!isChanged)
                //_curWeapon = WeaponEnum.None;
        }

        public WeaponEnum GetCurrentWeapon()
        {
            return _curWeapon;
        }

        public WeaponAction GetCurrentWeaponAction()
        {
            return _weaponActions[_curWeapon];
        }
    }
}
