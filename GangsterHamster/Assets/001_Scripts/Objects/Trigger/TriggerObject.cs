using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obejcts.Trigger
{
    /// <summary>
    /// 트리거 오브젝트
    /// </summary>
    abstract public class TriggerObject : MonoBehaviour, ITrigger
    {
        public int ID { get; set; } = -1;
        public bool Activated { get; set; } = false;
        public Action<GameObject> OnTrigger { get; set; }

        protected virtual void Awake()
        {
            TriggerManager.Instance.AddTrigger(this);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(!Activated) return;

            if(this.ID == -1)
            {
                Logger.Log($"TriggerObject > 등록되지 않은 트리거 {gameObject.name}", LogLevel.Error);
                return;
            }

            OnTrigger(other.gameObject);
        }

    }
}