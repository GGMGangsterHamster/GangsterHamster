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

        [SerializeField] private bool _isUsedFirstWeapon = false;
        [SerializeField] private bool _isUsedSecondWeaopn = false;
        [SerializeField] private bool _isUsedThirdWeapon = false;

        private const int firstWeaponNumber = 1;
        public int lastWeaponNumber = 0; // ���Ŀ� ���⸦ ������ �������� �þ��

        private Transform rightHandTrm;

        private void Awake()
        {
            curWeaponNumber = 0;

            if(isDeveloper)
            {
                lastWeaponNumber = 3;
                _isUsedFirstWeapon = true;
                _isUsedSecondWeaopn = true;
                _isUsedThirdWeapon = true;
            }

            _weaponDict.Add(-1, new FirstWeapon(null, null)); // ���̿�
            _weaponDict.Add(1, new FirstWeapon(gameObject, _firstWeaponSkill));
            _weaponDict.Add(2, new SecondWeapon(gameObject, _secondWeaponSkill));
            _weaponDict.Add(3, new ThirdWeapon(gameObject, _thirdWeaponSkill));

            _weaponDict[-1].isActive = false;
            _weaponDict[1].isActive = _isUsedFirstWeapon;
            _weaponDict[2].isActive = _isUsedSecondWeaopn;
            _weaponDict[3].isActive = _isUsedThirdWeapon;

            rightHandTrm = Define.RightHandTrm;
        }

        private void Update()
        {
            if(lastWeaponNumber != 0)
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

            if(_weaponDict[1].isActive != _isUsedFirstWeapon)
            {
                _weaponDict[1].isActive = _isUsedFirstWeapon;

                curWeaponNumber = -1;
                SetActiveWeapons(-1);
            }
            else if (_weaponDict[2].isActive != _isUsedSecondWeaopn)
            {
                _weaponDict[2].isActive = _isUsedSecondWeaopn;

                curWeaponNumber = -1;
                SetActiveWeapons(-1);
            }
            else if (_weaponDict[3].isActive != _isUsedThirdWeapon)
            {
                _weaponDict[3].isActive = _isUsedThirdWeapon;

                curWeaponNumber = -1;
                SetActiveWeapons(-1);
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

            ChangeWeaponNumberByKey(lastWeaponNumber);
        }

        /// <summary>
        /// ������ weaponNumber�� ���� ���⸦ ��ȯ�����ִ� �Լ� (Alpha~���Ŀ��� ���)
        /// </summary>
        /// <param name="weaponNumber">�ٲ� ������ Number</param>
        private void ChangeWeaponNumberByKey(int weaponNumber)
        {
            if (lastWeaponNumber >= weaponNumber && _weaponDict[weaponNumber].isActive)
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
            int beforeNumber = addNumber;

            if (_weaponDict.TryGetValue(curWeaponNumber + addNumber, out WeaponCommand wc))
            {
                curWeaponNumber += addNumber;
            }
            else
            {
                curWeaponNumber = addNumber > 0 ? firstWeaponNumber : lastWeaponNumber;
            }

            if (!_weaponDict[curWeaponNumber + addNumber].isActive)
            {
                curWeaponNumber = beforeNumber;
                return;
            }

            SetActiveWeapons(curWeaponNumber);
        }

        private void SetActiveWeapons(int weaponNumber)
        {
            switch(weaponNumber)
            {
                case -1:
                    _firstWeaponSkill.gameObject.SetActive(false);
                    _secondWeaponSkill.gameObject.SetActive(false);
                    _thirdWeaponSkill.gameObject.SetActive(false);
                    break;
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

        public void SetActiveWeapon(int inWeaponNumber, bool isActive)
        {
            _weaponDict[inWeaponNumber].isActive = isActive;
        }
    }
}
