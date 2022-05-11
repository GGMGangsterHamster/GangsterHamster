using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Commands.Weapon
{
    public class ThirdWeapon : WeaponCommand
    {
        ThirdWeaponSkill _skill;
        public ThirdWeapon(ThirdWeaponSkill skill)
        {
            _skill = skill;
        }

        /// <summary>
        /// 좌클릭시 무기 발사
        /// </summary>
        public override void Left()
        {
            _skill.Shot(MainCamTrm.forward);
        }

        /// <summary>
        /// 우클릭시 능력 발동
        /// </summary>
        public override void Right()
        {
            _skill.ChangeGravity();
        }

        /// <summary>
        /// R키 클릭시 무기 리셋
        /// </summary>
        public override void Reset()
        {
            _skill.ComeBack();
        }
    }
}

