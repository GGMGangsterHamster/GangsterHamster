using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Commands.Weapon
{
    public class FirstWeapon : WeaponCommand
    {
        FirstWeaponSkill _skill;
        public FirstWeapon(FirstWeaponSkill skill)
        {
            _skill = skill;
        }

        /// <summary>
        /// ��Ŭ���� �Լ� ���� <br/>
        /// �ٶ󺸴� �������� ���Ⱑ ����ؼ� ���ư���!
        /// </summary>
        public override void Left()
        {
            _skill.Shot(MainCamTrm.forward);
        }

        /// <summary>
        /// ��Ŭ���� �Լ� ���� <br/>
        /// �÷��̾� �������� ���Ⱑ �̵��Ѵ�!
        /// </summary>
        public override void Right()
        {
            _skill.MoveToDestination();
        }

        /// <summary>
        /// RŰ�� ���� �� �Լ� ���� <br/>
        /// �÷��̾��� ���������� ���Ⱑ ���´�!
        /// </summary>
        public override void Reset()
        {
            _skill.ComeBack();
        }
    }
}
