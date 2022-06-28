using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Checkpoint;
using Matters.Gravity;
using Weapon.Animation.GravitoAnimation;
using Objects.Interaction;

namespace Weapons.Actions
{
    public class Gravito : WeaponAction
    {
        enum GravityDir
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            FORWARD,
            BACK
        }

        public float gravityChangeTime;
        public float alphaToZeroSpeed;
        public float dropPointAlphaDistance;

        public Vector3 GravitoHandPosition => HandPosition - (PlayerBaseTransform.right / 4);

        private Dictionary<GravityDir, Vector3> _gravityDirDict = new Dictionary<GravityDir, Vector3>();
        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle; 
        private CheckpointManager _checkpoint;
        private RaycastHit _aTypeHit;
        private Vector3 _currentChangeGravityDir;
        private Transform _aTypeTrm;
        private Vector3 _aTypeCurPos;
        private float _aTypeBeforeSize;
        private float _aTypeCurSize;
        private Transform _dropPoint;
        private GravityAffectedObject _playerGravity;

        private GravitoAnimator _gravitoAnimator;
        private WeaponManagement _weaponManagement;

        private CheckpointManager Checkpoint
        {
            get
            {
                if(_checkpoint == null)
                {
                    _checkpoint = FindObjectOfType<CheckpointManager>();
                }

                return _checkpoint;
            }
        }

        private float _currentGravityChangeTime = 0f;  
        private bool isChangedGravity = false;
        private bool isReseting = false;

        private float _alpha;

        private new void Awake()
        {
            base.Awake();

            _weaponEnum = WeaponEnum.Gravito;

            _gravityDirDict.Add(GravityDir.UP, Vector3.up);
            _gravityDirDict.Add(GravityDir.DOWN, Vector3.down);
            _gravityDirDict.Add(GravityDir.LEFT, Vector3.left);
            _gravityDirDict.Add(GravityDir.RIGHT, Vector3.right);
            _gravityDirDict.Add(GravityDir.FORWARD, Vector3.forward);
            _gravityDirDict.Add(GravityDir.BACK, Vector3.back);

            _dropPoint = transform.GetChild(0);
            _dropPoint.parent = WeaponObjectParentTransform;

            _playerGravity = PlayerBaseTransform.GetComponent<GravityAffectedObject>();

            _gravitoAnimator = GetComponent<GravitoAnimator>();
            _weaponManagement = GameObject.FindObjectOfType<WeaponManagement>();

            _gravitoAnimator.sticklyAction += () =>
            {
                if(_currentGravitoStatus == GravitoStatus.Stickly && _gravitoAnimator.isStopedMoving())
                {
                    if (!_dropPoint.gameObject.activeSelf)
                    {
                        _dropPoint.gameObject.SetActive(true);

                        _alpha = 1;
                        Color temp = _dropPoint.GetComponent<MeshRenderer>().material.color;
                        _dropPoint.GetComponent<MeshRenderer>().material.color = new Color(temp.r, temp.g, temp.b, 1);
                        _dropPoint.rotation = Quaternion.LookRotation(-_aTypeHit.normal) * Quaternion.LookRotation(Vector3.up);
                    }
                }
            };
        }

        public override void FireWeapon()
        {
            if (_currentGravitoStatus == GravitoStatus.Idle &&
               !isReseting && _gravitoAnimator.isStopedMoving())
            {
                if (InteractionManager.Instance.currentRaycastHitTrm != null 
                    && InteractionManager.Instance.currentRaycastHitTrm.CompareTag("ATYPEOBJECT"))
                {
                    RaycastHit hit = InteractionManager.Instance.currentRaycastHit;

                    _fireDir = MainCameraTransform.forward;
                    _currentGravitoStatus = GravitoStatus.Stickly;
                    transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0);
                    _gravitoAnimator.FireAnime(transform.position, hit.point - (_fireDir * transform.localScale.y), fireSpeed);

                    _aTypeHit = hit;
                    _aTypeTrm = hit.transform;
                    _aTypeCurPos = hit.transform.position - hit.point;
                    _aTypeBeforeSize = hit.transform.localScale.x;
                    _aTypeCurSize = hit.transform.localScale.x;
                    _currentChangeGravityDir = CheckDir(hit.normal);
                }
            }
        }

        public override void UseWeapon()
        {
            if(_currentGravitoStatus == GravitoStatus.Stickly && !isChangedGravity && _gravitoAnimator.isStopedMoving())
            {
                if (_currentChangeGravityDir == Vector3.up) return;

                _gravitoAnimator.UsingAnime();

                _currentGravitoStatus = GravitoStatus.ChangeGravity;
                _currentGravityChangeTime = 0f;
                isChangedGravity = true;

                Checkpoint.SetStartCheckpoint(PlayerBaseTransform.forward);
                Checkpoint.SetEndCheckpoint(_currentChangeGravityDir);

                GravityManager.ChangeGlobalGravityDirection(-_currentChangeGravityDir);
                _playerGravity.AffectedByGlobalGravity = true;

                _dropPoint.rotation = Quaternion.identity;
            }
        }

        public override void ResetWeapon()
        {
            if (isReseting || _currentGravitoStatus == GravitoStatus.Idle || !_gravitoAnimator.isStopedMoving())
                return;

            _gravitoAnimator.ResetAnime(transform.position, GravitoHandPosition, fireSpeed);

            if (!isChangedGravity)
            {
                _currentGravitoStatus = GravitoStatus.Idle;
                _aTypeTrm = null;
                Update(); 
                
                if (_weaponManagement.GetCurrentWeapon() != _weaponEnum)
                {
                    gameObject.SetActive(false);
                }
                return;
            }

            _currentGravitoStatus = GravitoStatus.Reset;
            _currentGravityChangeTime = 0f;
            isChangedGravity = false;
            isReseting = true;

            Checkpoint.startCheckpoint.rotation = PlayerBaseTransform.rotation;
            Checkpoint.endCheckpoint.rotation = Quaternion.Euler(new Vector3(0, PlayerBaseTransform.rotation.eulerAngles.y, 0));

            GravityManager.ChangeGlobalGravityDirection(Vector3.down);
            _playerGravity.AffectedByGlobalGravity = true;

            Update();
        }

        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }
        public override Transform SticklyTrm()
        {
            return _aTypeTrm;
        }

        private void Update()
        {
            switch (_currentGravitoStatus)
            {
                case GravitoStatus.Idle:
                    ResetDropPoint();
                    break;
                case GravitoStatus.Stickly:
                    SettingGravitoPos();

                    if(isChangedGravity)
                    {
                        ShowDropPoint(Vector3.down);
                    }
                    else
                    {
                        ShowDropPoint(-_currentChangeGravityDir);
                    }
                    break;
                case GravitoStatus.ChangeGravity:
                    SettingGravitoPos();
                    ShowDropPoint(Vector3.down);
                    _currentGravityChangeTime += Time.deltaTime / gravityChangeTime;

                    if (_currentGravityChangeTime >= 1f)
                    {
                        PlayerBaseTransform.rotation = Checkpoint.endCheckpoint.rotation;
                        _currentGravitoStatus = GravitoStatus.Stickly;
                    }
                    else
                    {
                        PlayerBaseTransform.rotation = Quaternion.Lerp(
                            Checkpoint.startCheckpoint.rotation,
                            Checkpoint.endCheckpoint.rotation,
                            _currentGravityChangeTime);
                    }
                    break;
                case GravitoStatus.Reset:
                    SettingGravitoPos();
                    ShowDropPoint(Vector3.down);
                    _currentGravityChangeTime += Time.deltaTime / gravityChangeTime;

                    if(_currentGravityChangeTime >= 1f)
                    {
                        PlayerBaseTransform.rotation = Checkpoint.endCheckpoint.rotation;
                        _currentGravitoStatus = GravitoStatus.Idle;
                        transform.rotation = Quaternion.identity;
                        isReseting = false;
                        _aTypeTrm = null;
                        
                        if (_weaponManagement.GetCurrentWeapon() != _weaponEnum)
                        {
                            gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        PlayerBaseTransform.rotation = Quaternion.Lerp(
                            Checkpoint.startCheckpoint.rotation,
                            Checkpoint.endCheckpoint.rotation,
                            _currentGravityChangeTime);
                    }
                    break;
            }

        }

        private Vector3 CheckDir(Vector3 dir)
        {
            GravityDir gravityDir = GravityDir.UP;

            for(int i = 0; i < 6; i++)
            {
                if(Vector3.Angle(_gravityDirDict[(GravityDir)i], dir) <= 45)
                {
                    gravityDir = (GravityDir)i;
                    break;
                }
            }

            return _gravityDirDict[gravityDir];
        }

        private void SettingGravitoPos()
        {
            if (_aTypeCurPos != _aTypeTrm.position - _aTypeHit.point)
            {
                transform.position -= _aTypeCurPos - ((_aTypeTrm.position - _aTypeHit.point));
                _aTypeCurPos = _aTypeTrm.position - _aTypeHit.point;
            }

            if (_aTypeCurSize != _aTypeHit.transform.localScale.x)
            {
                _aTypeBeforeSize = _aTypeCurSize;
                _aTypeCurSize = _aTypeHit.transform.localScale.x;
                transform.position -= _aTypeHit.normal * (_aTypeBeforeSize - _aTypeCurSize) / 2;
            }
        }
        private void ShowDropPoint(Vector3 dir)
        {
            RaycastHit[] hits = Physics.RaycastAll(PlayerBaseTransform.position + PlayerBaseTransform.up, dir);
            float minDistance = float.MaxValue;
            int index = -1;

            if (hits != null)
            {

                for (int i = 0; i < hits.Length; i++)
                {
                    if ((hits[i].transform.CompareTag("BTYPEOBJECT") || hits[i].transform.CompareTag("ATYPEOBJECT")) && hits[i].distance < minDistance)
                    {
                        if(hits[i].transform.GetComponent<GravityAffectedObject>() == null || hits[i].transform.GetComponent<GravityAffectedObject>().AffectedByGlobalGravity)
                        {
                            minDistance = hits[i].distance;
                            index = i;
                        }
                    }
                }

                if (index != -1)
                {
                    RaycastHit hit = hits[index]; // 가장 가까운 바닥

                    _dropPoint.position = hit.point + -dir * 0.1f;


                    Color temp = _dropPoint.GetComponent<MeshRenderer>().material.color;
                    _dropPoint.GetComponent<MeshRenderer>().material.color = new Color(temp.r, temp.g, temp.b, Mathf.Clamp(Vector3.Distance(MainCameraTransform.position, hit.point) / dropPointAlphaDistance, 0, 1f));
                }
            }
        }
        private void ResetDropPoint()
        {
            if (!_dropPoint.gameObject.activeSelf) return;

            if (_alpha >= 0.2f)
            {
                _alpha -= Time.deltaTime * alphaToZeroSpeed;
                Color temp = _dropPoint.GetComponent<MeshRenderer>().material.color;
                _dropPoint.GetComponent<MeshRenderer>().material.color = new Color(temp.r, temp.g, temp.b, _alpha > 0.2f ? _alpha : 0);
            }
            else
            {
                _dropPoint.gameObject.SetActive(false);
            }
        }
    }
}