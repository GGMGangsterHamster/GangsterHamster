using System;
using Objects.Trigger;
using UI.PanelScripts;
using UnityEngine;


namespace Characters.Player
{
    public class DialogInteract : MonoBehaviour
    {
        private DialogVolume _curActiveVolume;

        private DialogPanel _panel;
        private DialogPanel Panel
        {
            get
            {
                if (_panel == null)
                    _panel = FindObjectOfType<DialogPanel>();
                return _panel;
            }
        }

        public void Set(DialogVolume volume, Action OnSet = null)
        {
            if ((_curActiveVolume == null)
             || (volume.priority >= _curActiveVolume.priority))
            {
                _curActiveVolume = volume;
                OnSet?.Invoke();
            }
        }

        public void Unset(DialogVolume volume, Action OnUnset = null)
        {
            if (volume == _curActiveVolume)
            {
                _curActiveVolume = null;
                OnUnset?.Invoke();
            }
        }

        public void Call()
        {
            if (_curActiveVolume == null) return;

            Panel.Show(_curActiveVolume.Get());
        }
    }
}