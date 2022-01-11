using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable.Object
{
    public class ObjectA : MonoBehaviour, IInteractableObject
    {
        public void Collision(GameObject collision, Action callback = null)
        {
            if(collision.CompareTag("WEAPON")) {
                // Collision with weapon
            }
            else {
                // something else
            }

            callback?.Invoke();
        }

        public virtual void Interact(Action callback = null)
        {
            

            callback?.Invoke();
        }
    }
}