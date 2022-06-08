using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Weapon.Animation.Lumo
{
    public class LumoAnimator : MonoBehaviour
    {
        enum LumoAnimeStatus // ���� �ִϸ��̼� �������ͽ�
        {
            Idle,
            Start,
            Using,
            Stop
        }

        [SerializeField] private float startAnimeDelayTime = 0.5f;                    // �ִϸ��̼��� �����ϴµ� ����� ������
        [SerializeField] private float stopAnimeDelayTime = 0.5f;                    // �ִϸ��̼��� ���ߴµ� ����� ������
        [SerializeField] private float rotSpeed = 1f;                                 // ���ư��� ȸ�� �ӵ�

        public UnityEvent StartAnimationCallback;
        public UnityEvent StopAnimationCallback;

        private List<Vector3> _partRotDirList = new List<Vector3>();    // ���������� �ִϸ��̼� ȸ�� ��
        private List<Transform> _partTrmList = new List<Transform>();
        private LumoAnimeStatus _curStatus = LumoAnimeStatus.Idle;    // ���� �ִϸ��̼� �������ͽ�

        private float _curTime;                                     // �ð� ����� ���� ����
        private int childCount;                                     // �ڽ��� �� - �׳� ���� ������ ���� ������ ��

        private void Awake()
        {
            childCount = transform.childCount;
            // ���۽� �������� ���������� ���� ȸ�� �� ����
            for (int i = 0; i < childCount; i++)
            {
                _partRotDirList.Add(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                _partTrmList.Add(transform.GetChild(i));
            }
        }

        public void StartAnimation()
        {
            _curTime = 0;
            _curStatus = LumoAnimeStatus.Start;
        }

        public void StopAnimation()
        {
            _curTime = stopAnimeDelayTime;
            _curStatus = LumoAnimeStatus.Stop;
        }

        private void Update()
        {
            #region �ӽ÷� ���� Input.GetKeyDown()
            if (Input.GetKeyDown(KeyCode.U))
                StartAnimation();
            if (Input.GetKeyDown(KeyCode.P))
                StopAnimation();
            #endregion

            // Idle �� �ƹ��͵� ���� �ʴ´�!
            switch (_curStatus)
            {
                case LumoAnimeStatus.Start:
                    if(_curTime >= startAnimeDelayTime)
                    {
                        // ���� �ִϸ��̼� ��
                        _curStatus = LumoAnimeStatus.Using;
                        StartAnimationCallback?.Invoke();
                    }
                    _curTime += Time.deltaTime;
                    RotationParts(_curTime / startAnimeDelayTime * Time.deltaTime * rotSpeed);

                    break;
                case LumoAnimeStatus.Stop:
                    if (_curTime <= 0)
                    {
                        // ��ž �ִϸ��̼� ��
                        _curStatus = LumoAnimeStatus.Idle;
                        StopAnimationCallback?.Invoke();
                    }
                    _curTime -= Time.deltaTime;
                    RotationParts(_curTime / stopAnimeDelayTime * Time.deltaTime * rotSpeed);

                    break;
                case LumoAnimeStatus.Using:
                    RotationParts(rotSpeed * Time.deltaTime);
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
    }
}