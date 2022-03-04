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
        public static int curWeaponNumber;

        private Dictionary<int, WeaponCommand> _weaponDict = new Dictionary<int, WeaponCommand>();

        // out 변수
        private WeaponCommand outwc;

        [SerializeField] private FirstWeaponSkill _firstWeaponSkill;
        [SerializeField] private SecondWeaponSkill _secondWeaponSkill;

        private const int firstWeaponNumber = 1;
        private int lastWeaponNumber = 0; // 추후에 무기를 얻으면 얻을수록 늘어난다

        private void Awake()
        {
            curWeaponNumber = 1;

            _weaponDict.Add(1, new FirstWeapon(gameObject, _firstWeaponSkill));
            _weaponDict.Add(2, new SecondWeapon(gameObject, _secondWeaponSkill));
        }

        private void Update()
        {
            if(curWeaponNumber != -1 && lastWeaponNumber != 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if(lastWeaponNumber >= 1)
                    {
                        curWeaponNumber = 1;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if(lastWeaponNumber >= 2)
                    {
                        curWeaponNumber = 2;
                    }
                }

                if (Input.mouseScrollDelta.y > 0) // 스크롤을 위로 굴렸을 때
                {
                    if (_weaponDict.TryGetValue(curWeaponNumber + 1, out WeaponCommand wc))
                    {
                        curWeaponNumber++;
                    }
                    else
                    {
                        curWeaponNumber = firstWeaponNumber;
                    }
                }
                else if (Input.mouseScrollDelta.y < 0) // 스크롤을 아래로 굴렸을 때
                {
                    if (_weaponDict.TryGetValue(curWeaponNumber - 1, out WeaponCommand wc))
                    {
                        curWeaponNumber--;
                    }
                    else
                    {
                        curWeaponNumber = lastWeaponNumber;
                    }
                }
            }
        }

        public void MouseLeft()
        {
            if (curWeaponNumber == -1) return;

            if(_weaponDict.TryGetValue(curWeaponNumber, out outwc))
            {
                outwc.Left();
            }
        }

        public void MouseRight()
        {
            if (curWeaponNumber == -1) return;

            if (_weaponDict.TryGetValue(curWeaponNumber, out outwc))
            {
                outwc.Right();
            }
        }

        public void R()
        {
            if (curWeaponNumber == -1) return;

            if (_weaponDict.TryGetValue(curWeaponNumber, out outwc))
            {
                outwc.Reset();
            }
        }
    }
}
