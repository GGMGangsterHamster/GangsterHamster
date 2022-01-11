using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable.Object
{
    public class ObjectB : MonoBehaviour, IInteractableObject
    {
        public virtual void Interact(Action callback = null)
        {
            callback?.Invoke();
        }
    }
}