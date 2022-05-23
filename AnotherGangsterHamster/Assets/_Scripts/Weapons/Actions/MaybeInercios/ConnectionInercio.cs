using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons.Actions
{
    public class ConnectionInercio : WeaponAction
    {
        public GameObject fireConnectionPrefab;
        public GameObject platformPrefab;
        private ConnectionStatus _currentStatus = ConnectionStatus.Idle;

        private GameObject _firstFireConnectionObj;
        private GameObject _secondFireConnectionObj;
        private GameObject _platformObj;

        private MeshRenderer _myRenderer;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Lumo;

            _firstFireConnectionObj = Instantiate(fireConnectionPrefab);
            _firstFireConnectionObj.name = "fireConnection-1";

            _secondFireConnectionObj = Instantiate(fireConnectionPrefab);
            _secondFireConnectionObj.name = "fireConnection-2";

            _platformObj = Instantiate(platformPrefab);
            _platformObj.name = "Platform";

            _myRenderer = GetComponent<MeshRenderer>();
        }
        public override void FireWeapon()
        {
            _fireDir = MainCameraTransform.forward;

            if (_currentStatus == ConnectionStatus.Idle)
            {
                if (Physics.Raycast(MainCameraTransform.position, _fireDir, out RaycastHit hit))
                {
                    _firstFireConnectionObj.transform.position = hit.point;
                    _firstFireConnectionObj.SetActive(true);
                    _currentStatus = ConnectionStatus.OneStickly;
                }
            }
            else if (_currentStatus == ConnectionStatus.OneStickly)
            {
                if (Physics.Raycast(MainCameraTransform.position, _fireDir, out RaycastHit hit))
                {
                    _secondFireConnectionObj.transform.position = hit.point;
                    _secondFireConnectionObj.SetActive(true);
                    _currentStatus = ConnectionStatus.TwoStickly;
                    _myRenderer.enabled = false;
                }
            }
        }

        public override void UseWeapon()
        {
            if(_currentStatus == ConnectionStatus.TwoStickly)
            {
                // 첫번째와 두번째 사이에 다리 생성

                Vector3 platformPos = _firstFireConnectionObj.transform.position -
                                        (_firstFireConnectionObj.transform.position - _secondFireConnectionObj.transform.position) / 2;

                _platformObj.transform.position = platformPos;
                _platformObj.transform.LookAt(_firstFireConnectionObj.transform.position);
                _platformObj.transform.localScale = new Vector3(3, 0.05f, Vector3.Distance(_firstFireConnectionObj.transform.position, _secondFireConnectionObj.transform.position));
                _platformObj.SetActive(true);


                _currentStatus = ConnectionStatus.Use;
            }
        }

        public override void ResetWeapon()
        {
            if(_currentStatus != ConnectionStatus.Idle)
            {
                _firstFireConnectionObj.SetActive(false);
                _secondFireConnectionObj.SetActive(false);
                _platformObj.SetActive(false);

                _currentStatus = ConnectionStatus.Idle;
                _myRenderer.enabled = true;
            }
        }

        public override bool IsHandleWeapon()
        {
            return _currentStatus == ConnectionStatus.Idle;
        }

        private void Update()
        {
            switch(_currentStatus)
            {
                case ConnectionStatus.Idle:
                    transform.position = HandPosition;
                    break;
                case ConnectionStatus.OneStickly:
                    transform.position = HandPosition;
                    break;
            }
        }
    }
}