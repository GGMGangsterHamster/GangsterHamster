using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.Trigger
{
    /// <summary>
    /// 트리거 오브젝트
    /// </summary>
    abstract public class TriggerObject : MonoBehaviour
    {
        public bool Activated { get; set; } = false;
        public Action<GameObject> OnTrigger { get; set; }

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnTrigger(other.gameObject);
        }

    }
}