using Objects.StageObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    [RequireComponent(typeof(CollisionInteractableObject))]
    public class Gravito : WeaponAction
    {
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
    }
}