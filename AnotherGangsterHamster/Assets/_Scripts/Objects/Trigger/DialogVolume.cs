using System.Collections.Generic;
using Characters.Player;
using Objects.InteractableObjects;
using UnityEngine;

namespace Objects.Trigger
{
    [RequireComponent(typeof(TriggerInteractableObject))]
    public class DialogVolume : MonoBehaviour, IEventable
    {
        public string type;

        public int idStart;
        public int idEnd;

        public int priority = 0;

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

        public void Active(GameObject other)
        {
            if (other.TryGetComponent<DialogInteract>(out var dialog))
            {
                dialog.Set(this);
            }
        }

        public void Deactive(GameObject other)
        {
            if (other.TryGetComponent<DialogInteract>(out var dialog))
            {
                dialog.Unset(this);
            }
        }

        public string Get()
        {
            if (_curId >= _dialogs.Count) return null;
            return _dialogs[_curId++].text;
        }
    }
}