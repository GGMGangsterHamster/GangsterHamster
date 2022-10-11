using System.Collections.Generic;
using Characters.Player;
using Objects.InteractableObjects;
using UnityEngine;
using _Core.Initialize;
using UI.Dialog;

namespace Objects.Trigger
{
    public class DialogVolume : MonoBehaviour, IEventable
    {
        private DialogPanel _panel;


        public string type = "";
        public int id = 0;

        public bool disableAfterActive = true;

        private void Start()
        {
            _panel = FindObjectOfType<DialogPanel>();
        }


        public void Active(GameObject other)
        {
            _panel.Display(type, id);

            gameObject.SetActive(!disableAfterActive);
        }

        public void Deactive(GameObject other)
        {
            // Do nothing
        }
    }
}