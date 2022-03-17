using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Commands.Weapon
{
    public class SecondWeapon : WeaponCommand
    {
        SecondWeaponSkill _skill;
        public SecondWeapon(SecondWeaponSkill skill)
        {
            _skill = skill;
        }

        /// <summary>
        /// 좌클릭시 함수 실행 <br/>
        /// 무기 발사
        /// </summary>
        public override void Left()
        {
            _skill.Shot(MainCamTrm.forward);
        }

        /// <summary>
        /// 우클릭시 함수 실행 <br/>
        /// 무기의 기믹 발동
        /// </summary>
        public override void Right()
        {
            _skill.ScaleUp();
        }

        /// <summary>
        /// R키를 누를 시 함수 실행 <br/>
        /// 무기 회수!
        /// </summary>
        public override void Reset()
        {
            _skill.ComeBack();
        }
    }
}
