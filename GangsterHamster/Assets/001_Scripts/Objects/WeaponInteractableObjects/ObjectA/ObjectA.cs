using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Interactable
{
    public class ObjectA : Interactable
    {
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

        }

        public override void Interact(Action callback = null)
        {
            

            callback?.Invoke();
        }

    }
}