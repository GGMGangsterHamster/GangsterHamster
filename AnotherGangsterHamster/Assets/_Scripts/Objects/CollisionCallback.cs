using System;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
   [Serializable]
   public class CollisionCallback
   {
      public string key;
      public UnityEvent<GameObject> OnActive = new UnityEvent<GameObject>();
      public UnityEvent<GameObject> OnDeactive = new UnityEvent<GameObject>();

      public CollisionCallback(UnityAction<GameObject> OnActive, UnityAction<GameObject> OnDeactive)
      {
         this.OnActive.AddListener(OnActive);
         this.OnDeactive.AddListener(OnDeactive);
      }
   }
}