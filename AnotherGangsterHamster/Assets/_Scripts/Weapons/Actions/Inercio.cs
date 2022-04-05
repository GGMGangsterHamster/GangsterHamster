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

        public override void FireWeapon()
        {

        }

        public override void UseWeapon()
        {

        }

        public override void ResetWeapon()
        {

        }
    }
}