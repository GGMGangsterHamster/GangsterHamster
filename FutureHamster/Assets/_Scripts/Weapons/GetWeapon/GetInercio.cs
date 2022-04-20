using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Actions;

namespace Weapons.GetWeapon
{
    public class GetInercio : MonoBehaviour
    {
        private Transform _weaponManagementTrm;
        private Inercio _inercio;

        private void Awake()
        {
            _weaponManagementTrm = FindObjectOfType<WeaponManagement>().transform;

            _inercio = _weaponManagementTrm.GetComponentInChildren<Inercio>();
        }

        public void PlayerBaseColliderEnter(GameObject obj)
        {
            _inercio.possibleUse = true;
            _inercio.ResetWeapon();

            gameObject.SetActive(false);
        }
    }
}
