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
        /// ��Ŭ���� �Լ� ���� <br/>
        /// ���� �߻�
        /// </summary>
        public override void Left()
        {
            _skill.Shot(_mainCamera.transform.forward);
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
            _skill.ComeBack(_rightHandTrm);
        }
    }
}
