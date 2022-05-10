using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class EggInercio : WeaponAction
    {
        private InercioStatus _currentInercioStatus = InercioStatus.Idle;

        private Transform _selectedTrm;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Inercio;
        }

        #region Actions
        public override void FireWeapon() // 오브젝트 선택
        {
            if (_currentInercioStatus == InercioStatus.Idle)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit, 100)
                    && hit.transform.CompareTag("ATYPEOBJECT"))
                {
                    _selectedTrm = hit.transform;

                    // Selected.
                    // TODO : Shader On (OutLine or Light On)

                    _currentInercioStatus = InercioStatus.Selected;
                }
            }
        }

        public override void UseWeapon() // 오브젝트와 위치 변환
        {
            if (_selectedTrm != null)
            {
                // Change Location

                Vector3 playerTrmPos = PlayerBaseTransform.position;
                PlayerBaseTransform.position = _selectedTrm.position - new Vector3(0, (_selectedTrm.localScale.y / 2), 0);
                _selectedTrm.position = playerTrmPos + new Vector3(0, (_selectedTrm.localScale.y / 2), 0);
            }
        }

        public override void ResetWeapon() // 오브젝트 선택 해제.
        {
            if(_currentInercioStatus != InercioStatus.Idle)
            {
                _selectedTrm = null;
                _currentInercioStatus = InercioStatus.Idle;
                gameObject.SetActive(true);
            }
        }

        public override bool IsHandleWeapon()
        {
            return _currentInercioStatus == InercioStatus.Idle;
        }
        #endregion

        private void Update()
        {
            switch (_currentInercioStatus)
            {
                case InercioStatus.Idle:
                    transform.position = HandPosition;
                    break;
                case InercioStatus.Selected:
                    // Active off
                    if (gameObject.activeSelf) gameObject.SetActive(false);
                    break;
            }
        }
    }
}
