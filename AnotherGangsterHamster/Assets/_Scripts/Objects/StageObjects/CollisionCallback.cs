using System;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.StageObjects
{
   [Serializable]
   public class CollisionCallback
   {
      public string key;
      public UnityEvent<GameObject> callback;
   }
}