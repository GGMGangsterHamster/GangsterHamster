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

      private int _idx = 0;

      private void Awake()
      {
         if (inputList.Count <= 0)
         {
            Logger.Log("InputChecker: List empty. Disabling checker.");
            this.enabled = false;
            return;
         }
      }

      private void Update()
      {
         Event e = Event.KeyboardEvent(Input.inputString);
         if (inputList[_idx].key == e.keyCode)
         {
            Debug.Log("Pressed: " + e.keyCode);
            inputList[_idx++].OnPressed?.Invoke();

            if (_idx >= inputList.Count)
            {
               Logger.Log("Input Check Completed. Disabling checker.");
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