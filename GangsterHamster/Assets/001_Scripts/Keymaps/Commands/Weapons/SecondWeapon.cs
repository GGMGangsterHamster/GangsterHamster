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
        /// ��Ŭ���� �Լ� ���� <br/>
        /// ���� �߻�
        /// </summary>
        public override void Left()
        {
            _skill.Shot(MainCamTrm.forward);
        }

        /// <summary>
        /// ��Ŭ���� �Լ� ���� <br/>
        /// ������ ��� �ߵ�
        /// </summary>
        public override void Right()
        {
            _skill.ScaleUp();
        }

        /// <summary>
        /// RŰ�� ���� �� �Լ� ���� <br/>
        /// ���� ȸ��!
        /// </summary>
        public override void Reset()
        {
            _skill.ComeBack();
        }
    }
}
