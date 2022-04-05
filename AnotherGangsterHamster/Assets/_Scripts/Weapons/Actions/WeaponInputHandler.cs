using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class WeaponInputHandler : MonoBehaviour
    {        
        private WeaponManagement _weaponManagement;

        private void Awake()
        {
            _weaponManagement = GetComponent<WeaponManagement>();
        }
    }
}