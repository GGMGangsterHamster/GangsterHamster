using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Characters.Player
{
   public class InputChecker : MonoBehaviour
   {
      public List<InputCheckObj> inputList
         = new List<InputCheckObj>();

      public UnityEvent OnCompleted;

      [field: SerializeField]
      public bool StepByStepInput { get; set; } = false;

      private int _idx = 0;

      private Func<KeyCode, bool> _compareable;

      private void Awake()
      {
         if (inputList.Count <= 0)
         {
            Logger.Log("InputChecker: List empty. Disabling checker.");
            this.enabled = false;
            return;
         }

         if (StepByStepInput)
         {
            _compareable = key
               => inputList[_idx].key == key;
         }
         else
         {
            _compareable = key
               => inputList.Find(obj => obj.key == key) != null;
         }
      }

      private void Update()
      {
         Event e = Event.KeyboardEvent(Input.inputString);

         if (e.isKey && _compareable(e.keyCode))
         {
            Debug.Log("Pressed: " + e.keyCode);
            inputList[_idx++].OnPressed?.Invoke();

            if (_idx >= inputList.Count) // 완료 시 비활성화
            {
               Logger.Log(
                  $"Check Completed for {inputList.Count} conditions."
                 + "Disabling checker.");
               OnCompleted?.Invoke();
               this.enabled = false;
               return;
            }
         }
      }
   }

   [Serializable]
   public class InputCheckObj
   {
      public UnityEvent OnPressed;
      public KeyCode key;
   }
}