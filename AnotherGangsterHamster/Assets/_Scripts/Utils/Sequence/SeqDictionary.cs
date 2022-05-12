using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sequence
{
   [Serializable]
   public class SeqDictionary
   {
      public string key;
      public List<SeqObject> seqObjects;
      public bool executeOnStart;
      public SequenceType type;
   }
}