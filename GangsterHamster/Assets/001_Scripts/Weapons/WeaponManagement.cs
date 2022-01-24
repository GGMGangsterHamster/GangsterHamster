using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Weapon
{
    public class WeaponManagement : MonoBehaviour, IWeaponable
    {
        /// <summary>
        /// 현재 어떤 무기를 들고 있나
        /// </summary>
        private static int curWeaponNumber;

        private Dictionary<int, WeaponCommand> _weaponDict = new Dictionary<int, WeaponCommand>();

        // out 변수
        private WeaponCommand outwc;

        [SerializeField]
        private FirstWeaponSkill _firstWeaponSkill;

        private void Awake()
        {
            curWeaponNumber = 1;

            _weaponDict.Add(1, new FirstWeapon(gameObject, _firstWeaponSkill));
        }
        public void MouseLeft()
        {
            if(_weaponDict.TryGetValue(curWeaponNumber, out outwc))
            {
                outwc.Left();
            }
        }

        public void MouseRight()
        {
            if (_weaponDict.TryGetValue(curWeaponNumber, out outwc))
            {
                outwc.Right();
            }
        }

        public void R()
        {
            if(_weaponDict.TryGetValue(curWeaponNumber, out outwc))
            {
                outwc.Reset();
            }
        }
    }
}
