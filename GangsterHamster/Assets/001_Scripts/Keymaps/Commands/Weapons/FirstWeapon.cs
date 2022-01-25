using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Weapon
{
    public class FirstWeapon : WeaponCommand
    {
        GameObject _playerObj;
        FirstWeaponSkill _skill;
        Camera _mainCamera;

        Transform _rightHandTrm;
        public FirstWeapon(GameObject playerObj, FirstWeaponSkill skill)
        {
            _playerObj = playerObj;
            _skill = skill;
            _mainCamera = Camera.main;

            _rightHandTrm = GameObject.Find("RightHand").transform;
        }

        /// <summary>
        /// 좌클릭시 함수 실행 <br/>
        /// 바라보는 방향으로 무기가 계속해서 나아간다!
        /// </summary>
        public override void Left()
        {
            _skill.Shot(_mainCamera.transform.forward);
        }

        /// <summary>
        /// 우클릭시 함수 실행 <br/>
        /// 플레이어 방향으로 무기가 이동한다!
        /// </summary>
        public override void Right()
        {
            _skill.MoveToDestination(_mainCamera.transform, _rightHandTrm, 1.5f);
        }

        /// <summary>
        /// R키를 누를 시 함수 실행 <br/>
        /// 플레이어의 오른손으로 무기가 들어온다!
        /// </summary>
        public override void Reset()
        {
            _skill.ComeBack(_rightHandTrm);
        }
    }
}
