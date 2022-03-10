using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Weapon
{
    public class ThirdWeapon : WeaponCommand
    {
        GameObject _playerObj;
        ThirdWeaponSkill _skill;
        Camera _mainCamera;

        Transform _rightHandTrm;
        public ThirdWeapon(GameObject playerObj, ThirdWeaponSkill skill)
        {
            _playerObj = playerObj;
            _skill = skill;
            _mainCamera = Camera.main;

            _rightHandTrm = GameObject.Find("RightHand").transform;
        }

        /// <summary>
        /// 좌클릭시 무기 발사
        /// </summary>
        public override void Left()
        {
            _skill.Shot(_mainCamera.transform.forward);
        }

        /// <summary>
        /// 우클릭시 능력 발동
        /// </summary>
        public override void Right()
        {
            //_skill.
        }

        /// <summary>
        /// R키 클릭시 무기 리셋
        /// </summary>
        public override void Reset()
        {
            _skill.Comeback(_rightHandTrm);
        }
    }
}

