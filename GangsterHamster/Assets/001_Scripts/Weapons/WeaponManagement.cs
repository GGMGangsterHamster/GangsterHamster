using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Commands.Weapon
{
    public class WeaponManagement : MonoBehaviour, IWeaponable
    {
        /// <summary>
        /// 현재 어떤 무기를 들고 있나
        /// </summary>
        public static int curWeaponNumber;
        public bool isDeveloper;

        private Dictionary<int, WeaponCommand> _weaponDict = new Dictionary<int, WeaponCommand>();

        // out 변수
        private WeaponCommand outwc;

        [SerializeField] private FirstWeaponSkill _firstWeaponSkill;
        [SerializeField] private SecondWeaponSkill _secondWeaponSkill;
        [SerializeField] private ThirdWeaponSkill _thirdWeaponSkill;

        [SerializeField] private bool _isUsedFirstWeapon = false;
        [SerializeField] private bool _isUsedSecondWeaopn = false;
        [SerializeField] private bool _isUsedThirdWeapon = false;

        private const int firstWeaponNumber = 1;
        public int lastWeaponNumber = 0; // 추후에 무기를 얻으면 얻을수록 늘어난다

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

            _weaponDict.Add(-1, new FirstWeapon(null, null)); // 더미용
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

                if (Input.mouseScrollDelta.y > 0) // 스크롤을 위로 굴렸을 때
                {
                    ChangeWeaponNumberByScroll(1);
                }
                else if (Input.mouseScrollDelta.y < 0) // 스크롤을 아래로 굴렸을 때
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
        /// 들어오는 weaponNumber에 따라 무기를 변환시켜주는 함수 (Alpha~형식에만 사용)
        /// </summary>
        /// <param name="weaponNumber">바꿀 무기의 Number</param>
        private void ChangeWeaponNumberByKey(int weaponNumber)
        {
            if (lastWeaponNumber >= weaponNumber && _weaponDict[weaponNumber].isActive)
            {
                curWeaponNumber = weaponNumber;
                SetActiveWeapons(weaponNumber);
            }
        }

        /// <summary>
        /// 들어오는 addNumber에 따라 다음 순서의 무기로 변환 시켜주는 함수(Scroll 형식에만 사용)
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
