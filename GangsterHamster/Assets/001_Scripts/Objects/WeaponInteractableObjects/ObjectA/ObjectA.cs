using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Interactable
{
    public class ObjectA : MonoBehaviour, IInteractableObject
    {
        public void Collision(GameObject collision, Action callback = null)
        {
            if(collision.CompareTag("WEAPON")) {
                transform.SetParent(collision.transform);
            }
            else {
                // something else
            }

            callback?.Invoke();
        }
        public virtual void Initialize(Action callback = null)
        {
            callback?.Invoke();
        }

        public virtual void Release()
        {

        }

        public virtual void Interact(Action callback = null)
        {
            

            callback?.Invoke();
        }

    }
}