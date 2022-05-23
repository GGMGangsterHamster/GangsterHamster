using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Weapons.Actions
{
    public class WeaponEvents : MonoSingleton<WeaponEvents>
    {
        public Action ChangedOneStep;
        public Action ChangedTwoStep;
        public Action ChangedMinSize;

        private new void Awake()
        {
            base.Awake();

            ChangedOneStep = () => { };
            ChangedTwoStep = () => { };
            ChangedMinSize = () => { };
        }
    }
}