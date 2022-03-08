using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Weapon
{
    public class WeaponManagement : MonoBehaviour, IWeaponable
    {
        /// <summary>
        /// ���� � ���⸦ ��� �ֳ�
        /// </summary>
        public static int curWeaponNumber;
        public bool isDeveloper;

        private Dictionary<int, WeaponCommand> _weaponDict = new Dictionary<int, WeaponCommand>();

        // out ����
        private WeaponCommand outwc;

        [SerializeField] private FirstWeaponSkill _firstWeaponSkill;
        [SerializeField] private SecondWeaponSkill _secondWeaponSkill;

        private const int firstWeaponNumber = 1;
        public int lastWeaponNumber = 0; // ���Ŀ� ���⸦ ������ �������� �þ��

        private Transform rightHandTrm;

        private void Awake()
        {
            curWeaponNumber = 0;

            if(isDeveloper)
            {
                lastWeaponNumber = 2;
            }

            _weaponDict.Add(1, new FirstWeapon(gameObject, _firstWeaponSkill));
            _weaponDict.Add(2, new SecondWeapon(gameObject, _secondWeaponSkill));

            rightHandTrm = GameObject.Find("RightHand").transform;
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
                        SetActiveWeapons(1);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if(lastWeaponNumber >= 2)
                    {
                        curWeaponNumber = 2;
                        SetActiveWeapons(2);
                    }
                }

                if (Input.mouseScrollDelta.y > 0) // ��ũ���� ���� ������ ��
                {
                    if (_weaponDict.TryGetValue(curWeaponNumber + 1, out WeaponCommand wc))
                    {
                        curWeaponNumber++;
                    }
                    else
                    {
                        curWeaponNumber = firstWeaponNumber;
                    }
                    SetActiveWeapons(curWeaponNumber);
                }
                else if (Input.mouseScrollDelta.y < 0) // ��ũ���� �Ʒ��� ������ ��
                {
                    if (_weaponDict.TryGetValue(curWeaponNumber - 1, out WeaponCommand wc))
                    {
                        curWeaponNumber--;
                    }
                    else
                    {
                        curWeaponNumber = lastWeaponNumber;
                    }
                    SetActiveWeapons(curWeaponNumber);
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

        public void SetMaxWeaponNumber(int maxNumber)
        {
            lastWeaponNumber = maxNumber;
            curWeaponNumber = maxNumber;
        }

        private void SetActiveWeapons(int weaponNumber)
        {
            switch(weaponNumber)
            {
                case 1:
                    _weaponDict[1].Reset();
                    _firstWeaponSkill.gameObject.SetActive(true);

                    if(_secondWeaponSkill.transform.parent == rightHandTrm)
                    {
                        _secondWeaponSkill.gameObject.SetActive(false);
                    }
                    break;
                case 2:
                    _weaponDict[2].Reset();
                    _secondWeaponSkill.gameObject.SetActive(true);

                    if (_firstWeaponSkill.transform.parent == rightHandTrm)
                    {
                        _firstWeaponSkill.gameObject.SetActive(false);
                    }
                    break;
                case 3:
                    _weaponDict[3].Reset();
                    break;
            }
        }
    }
}
