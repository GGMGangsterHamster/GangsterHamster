using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Interactable
{
    public class ObjectB : Interactable
    {
        public override void Collision(GameObject collision, Action callback = null)
        {
            callback?.Invoke();
        }

        public override void Release()
        {

        }

        public override void Interact(Action callback = null)
        {
            callback?.Invoke();
        }

        public override void Focus(Action callback = null) { }
        public override void DeFocus(Action callback = null) { }
    }
}