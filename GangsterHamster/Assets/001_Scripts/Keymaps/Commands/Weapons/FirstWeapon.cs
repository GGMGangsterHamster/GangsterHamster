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

        public override void Left()
        {
            _skill.Shot(_mainCamera.transform.forward);
        }

        public override void Right()
        {
            _skill.MoveToDestination(_playerObj.transform, _rightHandTrm, 1.5f);
        }

        public override void Reset()
        {
            _skill.ComeBack(_rightHandTrm);
        }
    }
}
