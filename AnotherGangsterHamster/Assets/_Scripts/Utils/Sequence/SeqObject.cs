using System;
using UnityEngine.Events;

namespace Sequence
{
   [Serializable]
   public class SeqObject
   {
      public UnityEvent Event;
      public float Delay = 0.0f;
   }
}