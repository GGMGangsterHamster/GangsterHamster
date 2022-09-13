using System;
using Objects.Trigger;
using UI.Dialog;
using UnityEngine;


namespace Characters.Player
{
    public class DialogInteract : MonoBehaviour
    {
        private DialogVolume _curActiveVolume;
        private DialogPanel _panelPrefab;

        private DialogPanel _panel;
        private DialogPanel Panel => _panel;

        private void Start()
        {
            _panelPrefab = FindObjectOfType<DialogPanel>();
            if (_panelPrefab == null)
            {
                _panelPrefab = Instantiate(
                                        Resources.Load<GameObject>("UI/cvsDialog")
                                        ).GetComponent<DialogPanel>();
            }
            
            _panel = _panelPrefab;
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
                Panel.Disable();
            }
        }

        public void Call()
        {
            if (_curActiveVolume == null)
            {
                Panel.Disable();
                _curActiveVolume = null;
                return;
            }

            InnerDialog dialog = _curActiveVolume.Get();

            if (dialog == null)
            {
                if (_curActiveVolume.disableWhenEnd)
                    Panel.Disable();
            }
            else
                Panel.Display(dialog.text);
        }
    }
}