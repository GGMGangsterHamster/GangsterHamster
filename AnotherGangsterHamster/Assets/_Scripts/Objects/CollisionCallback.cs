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

      public CollisionCallback(UnityAction<GameObject> OnActive, UnityAction<GameObject> OnDeactive)
      {
         this.OnActive.AddListener(OnActive);
         this.OnDeactive.AddListener(OnDeactive);
      }
   }
}