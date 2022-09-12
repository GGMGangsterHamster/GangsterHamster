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

        public List<GameObject> requirements
            = new List<GameObject>();

        public void Init()
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();
            List<Objects.IActivated> req = new List<Objects.IActivated>();

            requirements.ForEach(x => {
                req.Add(x.GetComponent<Objects.IActivated>());
            });

            CoroutineCaller
                .Instance
                .Use(() => !satisfied,
                    () => wait,
                    () => {
                        if (req.Find(x => !x.Activated) == null)
                            satisfied = true;
                    });
        }
    }
    
}