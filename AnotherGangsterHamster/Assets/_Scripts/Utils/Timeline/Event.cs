using System;
using UnityEngine;
using UnityEngine.Events;

namespace Timeline
{
    [Serializable]
    public enum Type
    {
        TIMED,
        EXECUTE_AFTER,
        EXECUTE_ON_SATISFIED,
    }

    [Serializable]
    public class Event
    {
        // public Type type = Type.TIMED;
        public string key = "";
        public bool satisfied = true;
        
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
    
}