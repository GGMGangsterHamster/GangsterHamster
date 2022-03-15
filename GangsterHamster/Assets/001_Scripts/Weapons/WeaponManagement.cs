using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
        [SerializeField] private ThirdWeaponSkill _thirdWeaponSkill;

        //[SerializeField] private bool _

        private const int firstWeaponNumber = 1;
        public int lastWeaponNumber = 0; // ���Ŀ� ���⸦ ������ �������� �þ��

        private Transform rightHandTrm;

        private void Awake()
        {
            curWeaponNumber = 0;

            if(isDeveloper)
            {
                lastWeaponNumber = 3;
            }

            _weaponDict.Add(1, new FirstWeapon(gameObject, _firstWeaponSkill));
            _weaponDict.Add(2, new SecondWeapon(gameObject, _secondWeaponSkill));
            _weaponDict.Add(3, new ThirdWeapon(gameObject, _thirdWeaponSkill));

            rightHandTrm = Define.RightHandTrm;
        }

        private void Update()
        {
            if(curWeaponNumber != -1 && lastWeaponNumber != 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    ChangeWeaponNumberByKey(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    ChangeWeaponNumberByKey(2);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    ChangeWeaponNumberByKey(3);
                }

                if (Input.mouseScrollDelta.y > 0) // ��ũ���� ���� ������ ��
                {
                    ChangeWeaponNumberByScroll(1);
                }
                else if (Input.mouseScrollDelta.y < 0) // ��ũ���� �Ʒ��� ������ ��
                {
                    ChangeWeaponNumberByScroll(-1);
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

        /// <summary>
        /// ������ weaponNumber�� ���� ���⸦ ��ȯ�����ִ� �Լ� (Alpha~���Ŀ��� ���)
        /// </summary>
        /// <param name="weaponNumber">�ٲ� ������ Number</param>
        private void ChangeWeaponNumberByKey(int weaponNumber)
        {
            if (lastWeaponNumber >= weaponNumber)
            {
                curWeaponNumber = weaponNumber;
                SetActiveWeapons(weaponNumber);
            }
        }

        /// <summary>
        /// ������ addNumber�� ���� ���� ������ ����� ��ȯ �����ִ� �Լ�(Scroll ���Ŀ��� ���)
        /// </summary>
        /// <param name="addNumber"></param>
        private void ChangeWeaponNumberByScroll(int addNumber)
        {
            if (_weaponDict.TryGetValue(curWeaponNumber + addNumber, out WeaponCommand wc))
            {
                curWeaponNumber += addNumber;
            }
            else
            {
                curWeaponNumber = addNumber > 0 ? firstWeaponNumber : lastWeaponNumber;
            }
            SetActiveWeapons(curWeaponNumber);
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
                    if(_thirdWeaponSkill.transform.parent == rightHandTrm)
                    {
                        _thirdWeaponSkill.gameObject.SetActive(false);
                    }

                    break;
                case 2:
                    _weaponDict[2].Reset();
                    _secondWeaponSkill.gameObject.SetActive(true);

                    if (_firstWeaponSkill.transform.parent == rightHandTrm)
                    {
                        _firstWeaponSkill.gameObject.SetActive(false);
                    }
                    if(_thirdWeaponSkill.transform.parent == rightHandTrm)
                    {
                        _thirdWeaponSkill.gameObject.SetActive(false);
                    }
                    break;
                case 3:
                    _weaponDict[3].Reset();
                    _thirdWeaponSkill.gameObject.SetActive(true);

                    if(_firstWeaponSkill.transform.parent == rightHandTrm)
                    {
                        _firstWeaponSkill.gameObject.SetActive(false);
                    }
                    if(_secondWeaponSkill.transform.parent == rightHandTrm)
                    {
                        _secondWeaponSkill.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }
}
