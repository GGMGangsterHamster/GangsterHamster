using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class WeaponInputHandler : MonoBehaviour
    {
        public string _path = "KeyCodes/Weapons";

        private WeaponManagement _weaponManagement;

        // 커맨드 패턴 아님
        private Dictionary<KeyCode, Action> _weaponCommand = new Dictionary<KeyCode, Action>();

        private void Awake()
        {
            _weaponManagement = GetComponent<WeaponManagement>();

            RemapCommands();
        }

        private void RemapCommands()
        {
            _weaponCommand.Clear();

            WeaponVO vo = Utils.JsonToVO<WeaponVO>(_path);

            _weaponCommand.Add((KeyCode)vo.Shot, () => _weaponManagement.FireCurrentWeapon());
            _weaponCommand.Add((KeyCode)vo.Use, () => _weaponManagement.UseCurrentWeapon());
            _weaponCommand.Add((KeyCode)vo.Reset, () => _weaponManagement.ResetCurrentWeapon());
            // 각각 1, 2, 3번 키를 누르면 그에 해당하는 무기로 변환하는거임
            _weaponCommand.Add((KeyCode)vo.ChangeToInercio, () => _weaponManagement.ChangeCurrentWeapon(WeaponEnum.Lumo));
            _weaponCommand.Add((KeyCode)vo.ChangeToGrand, () => _weaponManagement.ChangeCurrentWeapon(WeaponEnum.Grand));
            _weaponCommand.Add((KeyCode)vo.ChangeToGravito, () => _weaponManagement.ChangeCurrentWeapon(WeaponEnum.Gravito));
        }

        private void Update()
        {
            foreach (KeyCode key in _weaponCommand.Keys)
            {
                if (Time.timeScale == 0) return;

                if (Input.GetKeyDown(key))
                    _weaponCommand[key]();
            }
        }
    }
}