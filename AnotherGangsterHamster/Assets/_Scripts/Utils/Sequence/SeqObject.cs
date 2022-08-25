using System;
using UnityEngine.Events;

namespace Sequence
{
   [Serializable]
   public class SeqObject
   {
      public UnityEvent Event;

      [UnityEngine.Header("실행 후 딜레이")]
      public float Delay = 0.0f;
   }
}