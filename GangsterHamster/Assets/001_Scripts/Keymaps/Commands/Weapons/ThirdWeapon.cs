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
        /// ��Ŭ���� ���� �߻�
        /// </summary>
        public override void Left()
        {
            _skill.Shot(MainCamTrm.forward);
        }

        /// <summary>
        /// ��Ŭ���� �ɷ� �ߵ�
        /// </summary>
        public override void Right()
        {
            _skill.ChangeGravity();
        }

        /// <summary>
        /// RŰ Ŭ���� ���� ����
        /// </summary>
        public override void Reset()
        {
            _skill.ComeBack();
        }
    }
}

