using System.Collections.Generic;
using UnityEngine;

namespace Objects.StageObjects.CollisionEventable
{
    public class ActivateEvent : MonoBehaviour, ICollisionEventable
    {
        public bool startActive = true; // 처음 시작할때 어떤 상태로 시작 할 건지
        public bool activateMe = true;  // 자신을 Active Deactive 할 건지

        // 만약 activateMe 변수가 false라면 이걸 Active Deactive 시킨다.
        public List<GameObject> activateObjects = new List<GameObject>();

        private ButtonCountRequirement _requirement;

        private void Awake()
        {
            _requirement = GetComponent<ButtonCountRequirement>();

            _requirement.changedEvent += value =>
            {
                Debug.Log("In");
                if (value)
                    Active(null);
                else
                    Deactive(null);
            };

            if (startActive)
                Active(null);
            else
                Deactive(null);
        }

        public void Active(GameObject other)
        {
            if (_requirement == null || _requirement.Checked)
            {
                if(activateMe)
                    gameObject.SetActive(true);
                else
                {
                    foreach(GameObject obj in activateObjects)
                    {
                        obj.SetActive(true);
                    }
                }
            }
        }

        public void Deactive(GameObject other)
        {
            if(activateMe)
                gameObject.SetActive(false);
            else
            {
                Debug.Log("false");
                foreach (GameObject obj in activateObjects)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}