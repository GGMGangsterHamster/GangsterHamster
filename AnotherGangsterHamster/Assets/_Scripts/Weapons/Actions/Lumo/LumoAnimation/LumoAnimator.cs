using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Weapons.Actions;

namespace Weapon.Animation.LumoAnimation
{
    public class LumoAnimator : MonoBehaviour
    {
        private Transform _mainCameraTransform;
        private Transform MainCameraTransform
        {
            get
            {
                if (_mainCameraTransform == null)
                {
                    _mainCameraTransform = Camera.main.transform;
                }

                return _mainCameraTransform;
            }
        }

        enum LumoAnimeStatus // ���� �ִϸ��̼� �������ͽ�
        {
            Idle,
            Move,
            Reset,
            Using,
            Sorting
        }

        public float usingAnimeDelay = 0.5f;                    // �ִϸ��̼��� �����ϴµ� ����� ������
        public float sortPartsSpeed = 0.3f;                    // �ִϸ��̼��� ���ߴµ� ����� ������
        [SerializeField] private float rotSpeed = 1f;                                 // ���ư��� ȸ�� �ӵ�

        public UnityEvent StartAnimationCallback;
        public UnityEvent StopAnimationCallback;

        private List<Vector3> _partRotDirList = new List<Vector3>();    // ���������� �ִϸ��̼� ȸ�� ��
        private List<Transform> _partTrmList = new List<Transform>();
        private LumoAnimeStatus _curStatus = LumoAnimeStatus.Idle;    // ���� �ִϸ��̼� �������ͽ�

        private Quaternion quaternion;
        private Vector3 start;                                              // �ִϸ��̼� ����� ���� ��ġ
        private Vector3 end;                                                // �ִϸ��̼� ����� ������ ��ġ
        private float moveSpeed;                                            // �ִϸ��̼� ���� ���ǵ�
        
        private float _curTime;                                     // �ð� ����� ���� ����
        private float _softMoving;                                  // �ε巴�� �����̱� ���Ͽ� ����ϴ� ����
        private int childCount;                                     // �ڽ��� �� - �׳� ���� ������ ���� ������ ��
        private bool isEnd = false; 
        private bool isReset = true;

        private Lumo _lumo;

        private void Awake()
        {
            childCount = 9;

            // ���۽� �������� ���������� ���� ȸ�� �� ����
            for (int i = 0; i < childCount; i++)
            {
                _partRotDirList.Add(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                _partTrmList.Add(transform.GetChild(i));
            }

            _lumo = GetComponent<Lumo>();
        }

        public void FireAnime(Vector3 start, Vector3 end, float moveSpeed, Quaternion quaternion)
        {
            // Sorting -> Move -> Idle �� ���� ��ȯ
            InitAnime(start, end, moveSpeed, LumoAnimeStatus.Move, quaternion);
            isReset = false;
            isEnd = false;
        }
        
        public void ResetAnime(Vector3 start, Vector3 end, float moveSpeed)
        {
            // Sorting -> Move -> Idle �� ���� ��ȯ
            InitAnime(start, end, moveSpeed, LumoAnimeStatus.Sorting, Quaternion.Euler(new Vector3(90, 0, 0)));
            isReset = true;
            isEnd = false;
        }

        public void UsingAnime(Vector3 normalVec, float moveSpeed)
        {
            // Using -> ��� ������
            InitAnime(transform.position, transform.position + normalVec, moveSpeed, LumoAnimeStatus.Using, Quaternion.identity);
            _softMoving = 0f;
            isEnd = false;
        }

        public bool isStopedMoving()
        {
            return _curStatus == LumoAnimeStatus.Idle || isEnd;
        }

        private void Update()
        {
            switch (_curStatus)
            {
                case LumoAnimeStatus.Idle:
                    if(isReset)
                    {
                        transform.position = _lumo.GetHandPos;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MainCameraTransform.forward), 0.5f);
                    }
                    break;
                case LumoAnimeStatus.Sorting:
                    _curTime += Time.deltaTime;

                    if (_curTime > sortPartsSpeed)
                    {
                        // �ൿ �� ���� �Ϸ�
                        _curStatus = LumoAnimeStatus.Reset;

                        _curTime = 0;

                        Debug.Log(quaternion.eulerAngles);

                        for (int i = 0; i < childCount; i++)
                        {
                            _partTrmList[i].localRotation = quaternion;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < childCount; i++)
                        {
                            _partTrmList[i].localRotation = Quaternion.Lerp(_partTrmList[i].localRotation, quaternion, _curTime / sortPartsSpeed);
                        }
                    }
                    break;
                case LumoAnimeStatus.Move:
                    _curTime += Time.deltaTime * moveSpeed;

                    if (_curTime >= Vector3.Distance(start, end))
                    {
                        // �߻� ��ġ�� �̵� �Ϸ�
                        transform.position = end;
                        _curStatus = LumoAnimeStatus.Idle;
                        _curTime = 0;
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(start, end, _curTime / Vector3.Distance(start, end));
                    }
                    break;
                case LumoAnimeStatus.Reset:
                    _curTime += Time.deltaTime * moveSpeed;

                    if (_curTime >= Vector3.Distance(start, _lumo.GetHandPos))
                    {
                        // �߻� ��ġ�� �̵� �Ϸ�
                        transform.position = _lumo.GetHandPos;
                        _curStatus = LumoAnimeStatus.Idle;
                        _curTime = 0;
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(start, _lumo.GetHandPos, _curTime / Vector3.Distance(start, _lumo.GetHandPos));
                    }
                    break;

                case LumoAnimeStatus.Using:
                    _curTime += Time.deltaTime / usingAnimeDelay * _softMoving;
                    _softMoving += Time.deltaTime;

                    if (_curTime >= Vector3.Distance(start, end))
                    {
                        // ���� Ƣ���
                        RotationParts(rotSpeed * Time.deltaTime);
                        isEnd = true;
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(start, end, _curTime / Vector3.Distance(start, end));
                    }
                    break;
            }
        }

        // ���� ���ǵ忡 ���� �������� ������ �Լ�
        private void RotationParts(float rotSpeed)
        {
            for(int i = 0; i < childCount; i++)
            {
                _partTrmList[i].rotation *= Quaternion.Euler(_partRotDirList[i] * rotSpeed);
            }
        }

        private void InitAnime(Vector3 start, Vector3 end, float moveSpeed, LumoAnimeStatus status, Quaternion quaternion)
        {
            _curTime = 0;

            this.start = start;
            this.end = end;
            this.moveSpeed = moveSpeed;
            this.quaternion = quaternion;
            _curStatus = status;
        }
    }
}