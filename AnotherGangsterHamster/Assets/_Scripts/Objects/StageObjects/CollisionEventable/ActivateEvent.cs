using System.Collections.Generic;
using UnityEngine;

namespace Objects.StageObjects.CollisionEventable
{
    public class ActivateEvent : MonoBehaviour, ICollisionEventable
    {
        public bool startActive = true; // ó�� �����Ҷ� � ���·� ���� �� ����
        public bool activateMe = true;  // �ڽ��� Active Deactive �� ����

        // ���� activateMe ������ false��� �̰� Active Deactive ��Ų��.
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