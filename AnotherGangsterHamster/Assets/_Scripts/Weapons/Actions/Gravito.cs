using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Gravito : WeaponAction
    {
        public float fireSpeed;
        public float gravityChangeSpeed;
        private enum GravitoStatus
        {
            Idle,
            Fire,
            Use,
            ChangeGravity,
        }

        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle;

        private void Awake()
        {
            _weaponEnum = WeaponEnum.Gravito;
        }

        public override void FireWeapon()
        {

        }

        public override void UseWeapon()
        {

        }

        public override void ResetWeapon()
        {

        }

        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }
    }
}