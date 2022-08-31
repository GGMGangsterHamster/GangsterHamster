using System.Collections.Generic;
using Characters.Player;
using Objects.InteractableObjects;
using UnityEngine;
using _Core.Initialize;

namespace Objects.Trigger
{
    [RequireComponent(typeof(TriggerInteractableObject))]
    public class DialogVolume : MonoBehaviour, IEventable
    {
        public string type;

        public int idStart;
        public int idEnd;
        public bool disableWhenEnd = true;

        public int priority = 0;

        public bool callWhenTriggered = false;
        public bool doNotDisableWhenExit = false;

        private List<InnerDialog> _dialogs
            = new List<InnerDialog>();

        private int _curId = 0;

        private void Start()
        {
            for (int i = idStart; i <= idEnd; ++i)
            {
                _dialogs.Add(DialogManager.Instance.GetDialog(type, i));
            }
        }

        /// <summary>
        /// OnEnterTrigger 할 시 호출됨
        /// </summary>
        /// <param name="other">Triggered Gameobject</param>
        public void Active(GameObject other)
        {
            if (other.TryGetComponent<DialogInteract>(out var dialog))
            {
                dialog.Set(this);

                if (callWhenTriggered)
                    dialog.Call();
            }
        }

        /// <summary>
        /// OnExitTrigger 할 시 호출됨
        /// </summary>
        /// <param name="other">Triggered Gameobject</param>
        public void Deactive(GameObject other)
        {
            if (doNotDisableWhenExit) return;

            if (other.TryGetComponent<DialogInteract>(out var dialog))
            {
                dialog.Unset(this);
            }
        }

        public string GetByID(int id)
        {
            if (id > idEnd || id < 0) return null;
            return _dialogs[id].text;
        }

        public string Get()
        {
            if (_curId > idEnd) return null;
            return _dialogs[_curId++].text;
        }
    }
}