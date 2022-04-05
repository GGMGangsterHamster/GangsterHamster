using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Inercio : WeaponAction
    {
        private void Awake()
        {
            _weaponEnum = WeaponEnum.Inercio;
        }

        public override void ShotWeapon()
        {

        }

        public override void ActivateWeapon()
        {

        }

        public override void ResetWeapon()
        {

        }
    }
}