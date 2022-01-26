using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.UI.Management
{
    public class FloatingUIManager : MonoSingleton<FloatingUIManager>
    {
        [SerializeField] private FloatingUI floatingUI = null;
        
        /// <summary>
        /// $"{key} {msg}" 로 표시됨
        /// </summary>
        public void KeyHelper(KeyCode key, string msg, Transform pos)
        {
            floatingUI.Set($"{key} {msg}", pos);
        }

        /// <summary>
        /// $"{msg}" 로 표시됨
        /// </summary>
        public void Message(string msg, Transform pos)
        {
            floatingUI.Set(msg, pos);
        }

        public void DisableUI()
        {
            floatingUI.UnSet();
        }
    }
}