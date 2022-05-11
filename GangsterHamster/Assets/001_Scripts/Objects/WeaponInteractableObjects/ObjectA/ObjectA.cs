using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Interactable
{
    public class ObjectA : Interactable
    {
        /// <summary>
        /// �ε��� ���� Collision�Լ��� ȣ���ϸ� �� �ε��� ������Ʈ�� �θ� �ʤ���
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="callback"></param>
        public override void Collision(GameObject collision, Action callback = null)
        {
            if(collision.CompareTag("WEAPON")) {
                transform.SetParent(collision.transform);
            }
            else {
                // something else
            }

            callback?.Invoke();
        }
        
        public override void Release()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.parent = null;
        }

        public override void Interact(Action callback = null)
        {
            

            callback?.Invoke();
        }

        public override void Focus(Action callback = null) { }

        public override void DeFocus(Action callback = null) { }
    }
}