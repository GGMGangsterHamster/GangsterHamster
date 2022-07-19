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
   }
}