using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.UI.Management
{
   public class FloatingUIManager : MonoSingleton<FloatingUIManager>
   {
      private FloatingUI _floatingUI;
      private FloatingUI FloatingUI
      {
         get
         {
            if (_floatingUI == null)
            {
               _floatingUI =
                  Instantiate(Resources.Load("UI/FloatingUI") as GameObject).
                  GetComponent<FloatingUI>();
            }
            
#if UNITY_EDITOR
            NULL.Check(_floatingUI);
#endif

            return _floatingUI;
         }
      }

      /// <summary>
      /// $"{key} {msg}" 로 표시됨
      /// </summary>
      public void KeyHelper(KeyCode key, string msg, Transform pos)
      {
         FloatingUI.Set($"{key} {msg}", pos);
      }

      /// <summary>
      /// $"{msg}" 로 표시됨
      /// </summary>
      public void Message(string msg, Transform pos)
      {
         FloatingUI.Set(msg, pos);
      }

      public void DisableUI()
      {
         FloatingUI.UnSet();
      }
   }
}