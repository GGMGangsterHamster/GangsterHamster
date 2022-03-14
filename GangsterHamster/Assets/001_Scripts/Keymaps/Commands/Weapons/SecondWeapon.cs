using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Commands.Weapon
{
    public class SecondWeapon : WeaponCommand
    {
        GameObject _playerObj;
        SecondWeaponSkill _skill;
        Camera _mainCamera;

        Transform _rightHandTrm;
        public SecondWeapon(GameObject playerObj, SecondWeaponSkill skill)
        {
            _playerObj = playerObj;
            _skill = skill;
            _mainCamera = Camera.main;

            _rightHandTrm = Define.RightHandTrm;
        }

        /// <summary>
        /// 좌클릭시 함수 실행 <br/>
        /// 무기 발사
        /// </summary>
        public override void Left()
        {
            _skill.Shot(_mainCamera.transform.forward);
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
            _skill.ComeBack(_rightHandTrm);
        }
    }
}
