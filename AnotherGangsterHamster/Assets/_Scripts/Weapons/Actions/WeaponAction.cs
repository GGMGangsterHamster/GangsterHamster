using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    // 이너시오, 그랜드, 그래비토 3가지의 무기들을 관리하기 위한 부모 클래스이며
    // 공통된 기능들을 함수로 정리 해놓기도 하였다.
    public class WeaponAction : MonoBehaviour
    {
        public WeaponEnum _weaponEnum; // 상속받은 무기의 종류

        public bool possibleUse = false; // 사용이 가능한가

        // 기본적인 함수들
        public virtual void ShotWeapon()
        {
            // 1. 좌클릭으로 무기 발사
        }

        public virtual void ActivateWeapon()
        {
            // 2. 우클릭으로 능력 발동
        }

        public virtual void ResetWeapon()
        {
            // 3. R로 무기 회수
        }
    }
}