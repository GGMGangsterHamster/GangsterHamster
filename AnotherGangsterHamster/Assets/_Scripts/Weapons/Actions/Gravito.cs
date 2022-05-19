using Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Checkpoint;
using Matters.Gravity;

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
        public float penetratePadding;

        private Dictionary<GravityDir, Vector3> _gravityDirDict = new Dictionary<GravityDir, Vector3>();
        private GravitoStatus _currentGravitoStatus = GravitoStatus.Idle; 
        private CheckpointManager _checkpoint;
        private RaycastHit _aTypeHit;
        private Vector3 _currentChangeGravityDir;
        private Transform _aTypeTrm;
        private Vector3 _aTypeCurPos;
        private Transform _dropPoint;
        private LineRenderer _dropLineRenderer;

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
            _dropLineRenderer = transform.GetChild(1).GetComponent<LineRenderer>();

            _dropPoint.parent = WeaponObjectParentTransform;
            _dropLineRenderer.transform.parent = WeaponObjectParentTransform;
        }

        public override void FireWeapon()
        {
            if (_currentGravitoStatus == GravitoStatus.Idle &&
               !isReseting)
            {
                if(Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit) && hit.transform.CompareTag("ATYPEOBJECT"))
                {
                    _myRigid.constraints = RigidbodyConstraints.FreezeAll;
                    _fireDir = MainCameraTransform.forward;
                    _currentGravitoStatus = GravitoStatus.Stickly;
                    transform.position = hit.point - (_fireDir * (transform.localScale.y + penetratePadding));
                    transform.rotation = Quaternion.LookRotation(_fireDir) * Quaternion.Euler(90, 0, 0);

                    _aTypeHit = hit;
                    _aTypeTrm = hit.transform;
                    _aTypeCurPos = hit.transform.position - hit.point;
                    _currentChangeGravityDir = CheckDir(hit.normal);

                    // 일정 각도 차 만큼 돌리고 픔
                    // 차 값, 위치 이동 값
                    // 쿼터니언 끼리의 차를 구하고
                    // 그걸 계속해서 저장한 뒤
                    // 그게 변환되면
                    // 새로이 변환된 값을 이용해서 각도와 위치를 돌린다.

                    // 위치는 어떤 기준이 되는 방향 벡터를 기준으로
                    // 처음 각도를 할당한 뒤
                    // 나중에 들어온 값과 비교해서 그거의 차 만큼
                    // 얘를 돌려주면 됨

                    if (!_dropPoint.gameObject.activeSelf)
                    {
                        _dropPoint.gameObject.SetActive(true);
                        _dropLineRenderer.gameObject.SetActive(true);

                        _alpha = 1;
                        Color temp = _dropPoint.GetComponent<MeshRenderer>().material.color;
                        _dropPoint.GetComponent<MeshRenderer>().material.color = new Color(temp.r, temp.g, temp.b, 1);
                        _dropPoint.rotation = Quaternion.LookRotation(-_aTypeHit.normal) * Quaternion.LookRotation(Vector3.up);
                    }
                }
            }
        }

        public override void UseWeapon()
        {
            if(_currentGravitoStatus == GravitoStatus.Stickly && !isChangedGravity)
            {
                if (_currentChangeGravityDir == Vector3.up) return;

                _currentGravitoStatus = GravitoStatus.ChangeGravity;
                _currentGravityChangeTime = 0f;
                isChangedGravity = true;

                Checkpoint.SetStartCheckpoint(PlayerBaseTransform.forward);
                Checkpoint.SetEndCheckpoint(_currentChangeGravityDir);

                GravityManager.ChangeGlobalGravityDirection(-_currentChangeGravityDir);

                _dropPoint.rotation = Quaternion.identity;
            }
        }

        public override void ResetWeapon()
        {
            if (isReseting)
                return;

            _myRigid.constraints = RigidbodyConstraints.None;
            
            if (!isChangedGravity)
            {
                _currentGravitoStatus = GravitoStatus.Idle;
                transform.rotation = Quaternion.identity;
                Update();
                return;
            }

            _currentGravitoStatus = GravitoStatus.Reset;
            _currentGravityChangeTime = 0f;
            isChangedGravity = false;
            isReseting = true;

            Checkpoint.startCheckpoint.rotation = PlayerBaseTransform.rotation;
            Checkpoint.endCheckpoint.rotation = Quaternion.Euler(new Vector3(0, PlayerBaseTransform.rotation.eulerAngles.y, 0));
            
            GravityManager.ChangeGlobalGravityDirection(Vector3.down);

            Update();
        }

        public override bool IsHandleWeapon()
        {
            return _currentGravitoStatus == GravitoStatus.Idle;
        }

        private void Update()
        {
            switch (_currentGravitoStatus)
            {
                case GravitoStatus.Idle:
                    Vector3 gravitoHandPos = HandPosition - (PlayerBaseTransform.right / 4);

                    if(Vector3.Distance(transform.position, gravitoHandPos) > 1f)
                    {
                        transform.position = gravitoHandPos;
                    }
                    _myRigid.velocity = (gravitoHandPos - transform.position) * 20;
                    _myRigid.angularVelocity = _myRigid.angularVelocity / 2;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.5f);

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
                        ShowDropPoint(-_aTypeHit.normal);
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
                transform.position -= _aTypeCurPos - (_aTypeTrm.position - _aTypeHit.point);
                _aTypeCurPos = _aTypeTrm.position - _aTypeHit.point;
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
                        minDistance = hits[i].distance;
                        index = i;
                    }
                }

                if (index != -1)
                {
                    RaycastHit hit = hits[index]; // 가장 가까운 바닥

                    _dropPoint.position = hit.point + -dir * 0.1f;
                    _dropLineRenderer.transform.position = hit.point + -dir * (Vector3.Distance(hit.point, PlayerBaseTransform.position) / 2);

                    _dropLineRenderer.SetPosition(0, -dir * (Vector3.Distance(_dropLineRenderer.transform.position, hit.point) - 0.2f));
                    _dropLineRenderer.SetPosition(1, dir * Vector3.Distance(_dropLineRenderer.transform.position, hit.point));
                }
            }
        }
        private void ResetDropPoint()
        {
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
            _dropLineRenderer.gameObject.SetActive(false);
        }


    }
}