using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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

            _rightHandTrm = Define.RightHandTrm;
        }

        /// <summary>
        /// ��Ŭ���� ���� �߻�
        /// </summary>
        public override void Left()
        {
            _skill.Shot(_mainCamera.transform.forward);
        }

        /// <summary>
        /// ��Ŭ���� �ɷ� �ߵ�
        /// </summary>
        public override void Right()
        {
            //_skill.
        }

        /// <summary>
        /// RŰ Ŭ���� ���� ����
        /// </summary>
        public override void Reset()
        {
            _skill.Comeback(_rightHandTrm);
        }
    }
}

