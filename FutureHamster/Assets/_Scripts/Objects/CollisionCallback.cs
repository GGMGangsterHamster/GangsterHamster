using System;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
   [Serializable]
   public class CollisionCallback
   {
      public string key;
      public UnityEvent<GameObject> OnActive;
      public UnityEvent<GameObject> OnDeactive;
   }
}