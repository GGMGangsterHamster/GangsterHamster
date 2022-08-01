using System;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
   [Serializable]
   public class Event
   {
      public string key;
      public UnityEvent<GameObject> OnActive;
      public UnityEvent<GameObject> OnDeactive;

      public Event()
      {
         OnActive    = new UnityEvent<GameObject>();
         OnDeactive  = new UnityEvent<GameObject>();
      }

      public Event(string key,
                   UnityEvent<GameObject> OnActive,
                   UnityEvent<GameObject> OnDeactive)
      {
         this.OnActive   = OnActive;
         this.OnDeactive = OnDeactive;
         this.key        = key;
      }
   }
}