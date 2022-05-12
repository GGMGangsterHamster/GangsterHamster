using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sequence
{
   public class Sequencer : MonoBehaviour
   {
      public List<SeqDictionary> sequenceDictionary;

      public void Execute(string key, SequenceType type)
      {
         SeqDictionary seq = sequenceDictionary.Find(e => e.key == key);
         if (seq == null)
         {
            Logger.Log($"{key} 에 해당하는 Sequence 를 찾을 수 없습니다.",
                  LogLevel.Error);
            return;
         }

         // if (type == SequenceType.ONESHOT)

         // CoroutineCaller.Instance.Use();

      }
   }
}