using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Objects.Trigger;

namespace UI.Dialog
{
    public class DialogPanel : MonoBehaviour
    {
        public RectTransform dialogLocation;

        private GameObject _dialogObject;
        private List<DialogObject> _dialogPool
            = new List<DialogObject>();
        private Queue<DialogObject> _activeDialogPool
            = new Queue<DialogObject>();

        private void Start()
        {
            _dialogObject = Resources.Load("UI/DialogObject") as GameObject;

            for (int i = 0; i < 10; ++i)
                Add();
        }

        private DialogObject Add()
        {
            GameObject obj = Instantiate(_dialogObject, this.transform);

            obj.SetActive(false);
            obj.GetComponent<RectTransform>().position
                = dialogLocation.position;

            DialogObject panel = obj.GetComponent<DialogObject>();
            panel.gameObject.SetActive(false);
            _dialogPool.Add(panel);

            return panel;
        }

        public void Display(string text)
        {
            DialogObject obj
                = _dialogPool.Find(x => !x.gameObject.activeSelf);

            obj.Enable(text);
            _activeDialogPool.Enqueue(obj);
        }

        public void Disable()
        {
            if (_activeDialogPool.Count <= 0) return;

            _activeDialogPool
                .Dequeue()
                .Disable();
        }
    }
}