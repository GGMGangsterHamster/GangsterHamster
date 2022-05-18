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

      [field: SerializeField]
      public bool StepByStepInput { get; set; } = false;

      private int _idx = 0;

      private Func<bool> _compareable;

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
            _compareable = () => {
               return false;
            };
         }
         else
         {
            _compareable = () => {
               return false;
            };
         }
      }

      private void Update()
      {
         Event e = Event.KeyboardEvent(Input.inputString);

         if (inputList[_idx].key == e.keyCode)
         {
            Debug.Log("Pressed: " + e.keyCode);
            inputList[_idx++].OnPressed?.Invoke();

            if (_idx >= inputList.Count) // 완료 시 비활성화
            {
               Logger.Log(
                  $"Check Completed for {inputList.Count} conditions."
                 + "Disabling checker.");
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