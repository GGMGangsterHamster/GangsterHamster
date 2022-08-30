using System;
using Objects.Trigger;
using UI.PanelScripts;
using UnityEngine;


namespace Characters.Player
{
    public class DialogInteract : MonoBehaviour
    {
        private DialogVolume _curActiveVolume;
        private DialogPanel _panelPrefab;

        private DialogPanel _panel;
        private DialogPanel Panel
        {
            get
            {
                if (_panel == null)
                {
                    _panel = FindObjectOfType<DialogPanel>();
                    if (_panel == null)
                        _panel = _panelPrefab;
                }
                return _panel;
            }
        }

        private void Awake()
        {
            _panelPrefab = Instantiate(
                                    Resources.Load<GameObject>("UI/cvsDialog")
                                    ).GetComponent<DialogPanel>();

            _panelPrefab.Disable();
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

            string text = _curActiveVolume.Get();

            if (text == null)
                Panel.Disable();
            else
                Panel.Show(text);
        }
    }
}