using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Timeline
{
    [Serializable]
    public class Event
    {
        public string key = "";
        
        public UnityEvent actions
         = new UnityEvent();
    }

    [Serializable]
    public class TimedEvent : Event
    {
        public float delay = 0.0f;
    }

    [Serializable]
    public class ExecuteAfterEvent : TimedEvent
    {
        public string eventBefore = "";
    }

    [Serializable]
    public class ExecuteOnSatisfiedEvent : Event
    {
        [HideInInspector]
        public bool satisfied = false;

        public List<Objects.IActivated> requirements
            = new List<Objects.IActivated>();

        public void OnStatusChanged()
        {
            if (requirements.Find(x => !x.Activated) == null)
                satisfied = true;
        }
    }
    
}