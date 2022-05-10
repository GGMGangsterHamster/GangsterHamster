using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

namespace Weapons.Actions
{
    public class StopInercio : WeaponAction
    {
        private StopStatus _currentStopStatus = StopStatus.Idle;

        private new void Awake()
        {
            base.Awake();

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
            return _currentStopStatus == StopStatus.Idle;
        }

    }
}