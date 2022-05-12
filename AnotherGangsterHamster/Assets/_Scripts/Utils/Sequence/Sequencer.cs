using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sequence
{
   public class Sequencer : MonoBehaviour
   {
      public List<SeqDictionary> sequenceDictionary;

      private void Start()
      {
         var startSequence
            = sequenceDictionary.FindAll(e => e.executeOnStart);

         startSequence.ForEach(e => {
            Execute(e.key, e.type);
         });
      }

      public void Execute(string key, SequenceType type)
      {
         SeqDictionary seq = sequenceDictionary.Find(e => e.key == key);
         if (seq == null)
         {
            Logger.Log($"{key} 에 해당하는 Sequence 를 찾을 수 없습니다.",
                  LogLevel.Error);
            return;
         }

         // 클로져 시킬 용도
         int seqIdx = 0;
         bool loop = true;


         // 딜레이
         Func<YieldInstruction> wait
            = () => new WaitForSeconds(seq.seqObjects[seqIdx].Delay);

         // 매 loop 마다 실행할 것
         Action execute = () => {
            seq.seqObjects[seqIdx]
               .Event
              ?.Invoke();

            ++seqIdx;

            if (seqIdx >= seq.seqObjects.Count)
               seqIdx = 0;
         };

         // 루프 용
         Func<bool> keepGoing = () => loop;

         if (type == SequenceType.ONESHOT) // 한번만 실행하는 경우
            execute += () => loop = false;


         CoroutineCaller.Instance.Use(keepGoing, wait, execute);
      }
   }
}