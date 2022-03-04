using System.Collections;
using Objects.Interactable.Management;
using Objects.UI.Management;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Objects.Interactable
{
    public class OnOffButton : MonoBehaviour, IInteractableObject
    {
        public List<GameObject> mObjList = new List<GameObject>(); // On Off 할때 반응해야하는 오브젝트 리스트
        public Transform[] _uiPositions = new Transform[1];

        public Color OnColor;
        public Color OffColor;

        public float waitTime = 3f;

        private Material _myMat;
        private bool isReady = true;

        private void Awake()
        {
            _myMat = GetComponent<MeshRenderer>().material;
            _myMat.color = OffColor;
        }

        private enum OnOff
        {
            On,
            Off
        }

        [SerializeField]
        private OnOff status = OnOff.Off;

        public void Click()
        {
            if(!isReady)
            {
                return;
            }

            NextStatus();

            if (status == OnOff.On) // 켜지는 거 (On으로 되는거)
            {
                On();
            }
            else if (status == OnOff.Off) // 꺼지는 거 (Off으로 되는거)
            {
                Off();
            }
        }

        public virtual void On()
        {
            mObjList.ForEach(x => x.GetComponent<IActivateObject>().On());
        }

        public virtual void Off()
        {
            mObjList.ForEach(x => x.GetComponent<IActivateObject>().Off());
        }

        private void NextStatus()
        {
            StartCoroutine(TimerCo());

            if (status == OnOff.Off)
            {
                status = OnOff.On;
                _myMat.color = OnColor;
            }
            else
            {
                status += 1;
                _myMat.color = OffColor;
            }
        }

        IEnumerator TimerCo()
        {
            isReady = false;

            float curTime = 0f;

            while(true)
            {
                curTime += Time.deltaTime;

                if(curTime > waitTime) // 타이머 끝
                {
                    isReady = true;
                    yield break;
                }

                yield return null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PLAYER_BASE"))
            {
                FloatingUIManager.Instance.KeyHelper(KeyCode.E, "를 눌러 버튼을 활성화 시키세요.", GameManager.Instance.FindClosestPosition(_uiPositions));
                InteractionManager.Instance.SetInteraction(Click);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PLAYER_BASE"))
            {
                FloatingUIManager.Instance.DisableUI();
                InteractionManager.Instance.UnSetInteraction(Click);
            }
        }

        public void Interact(Action callback = null) { }
        public void Initialize(Action callback = null) { }
        public void Release() { }
        public void Collision(GameObject collision, Action callback = null) { }
    }
}
