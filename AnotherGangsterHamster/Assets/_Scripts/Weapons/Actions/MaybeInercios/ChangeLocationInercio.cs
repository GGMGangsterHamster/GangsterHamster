using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class ChangeLocationInercio : WeaponAction
    {
        private InercioStatus _currentInercioStatus = InercioStatus.Idle;

        private Transform _selectedTrmOne;
        private Transform _selectedTrmTwo;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Inercio;
        }

        #region Actions
        public override void FireWeapon() // ������Ʈ ����
        {
            if (_currentInercioStatus == InercioStatus.Idle)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit, 100)
                    && hit.transform.CompareTag("ATYPEOBJECT"))
                {
                    _selectedTrmOne = hit.transform;

                    // Selected.
                    // TODO : Shader On (OutLine or Light On)

                    _currentInercioStatus = InercioStatus.OneSelected;
                }
            }
            else if(_currentInercioStatus == InercioStatus.OneSelected)
            {
                if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit, 100)
                    && hit.transform.CompareTag("ATYPEOBJECT")
                    && hit.transform != _selectedTrmOne)
                {
                    _selectedTrmTwo = hit.transform;

                    // Selected.
                    // TODO : Shader On (OutLine or Light On)

                    _currentInercioStatus = InercioStatus.TwoSelected;
                }
            }
        }

        public override void UseWeapon() // ������Ʈ�� ��ġ ��ȯ
        {
            if (_currentInercioStatus == InercioStatus.TwoSelected)
            {
                // Change location

                Vector3 selectTrmTwoPos = _selectedTrmTwo.position;
                _selectedTrmTwo.position = _selectedTrmOne.position;
                _selectedTrmOne.position = selectTrmTwoPos;
            }
        }

        public override void ResetWeapon() // ������Ʈ ���� ����.
        {
            if (_currentInercioStatus != InercioStatus.Idle)
            {
                _selectedTrmOne = null;
                _selectedTrmTwo = null;
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
                case InercioStatus.OneSelected:
                    transform.position = HandPosition;
                    break;
                case InercioStatus.TwoSelected:
                    // Active off
                    if (gameObject.activeSelf) gameObject.SetActive(false);
                    break;
            }
        }
    }
}
