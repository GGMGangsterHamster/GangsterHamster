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
        /// 좌클릭시 함수 실행 <br/>
        /// 바라보는 방향으로 무기가 계속해서 나아간다!
        /// </summary>
        public override void Left()
        {
            _skill.Shot(MainCamTrm.forward);
        }

        /// <summary>
        /// 우클릭시 함수 실행 <br/>
        /// 플레이어 방향으로 무기가 이동한다!
        /// </summary>
        public override void Right()
        {
            _skill.MoveToDestination();
        }

        /// <summary>
        /// R키를 누를 시 함수 실행 <br/>
        /// 플레이어의 오른손으로 무기가 들어온다!
        /// </summary>
        public override void Reset()
        {
            _skill.ComeBack();
        }
    }
}
