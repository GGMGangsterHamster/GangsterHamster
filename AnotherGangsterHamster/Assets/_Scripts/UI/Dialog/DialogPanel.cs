using System.Collections.Generic;
using UnityEngine;
using Tween;

namespace UI.Dialog
{
    public class DialogPanel : MonoBehaviour
    {
        const float HALF_PI = Mathf.PI / 2.0f;

        public RectTransform dialogLocation;
        public float yPadding = 60.0f;
        public float yPushDuration = 0.1f;

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

            if (obj == null) // FIXME: 스팸할 경우 위치가 어긋남
                obj = Add();

            obj.Enable(text);

            if (_activeDialogPool.Count > 0)
            {
                DialogObject[] arr = _activeDialogPool.ToArray();

                for (int i = 0; i < arr.Length; ++i)
                {
                    arr[i].Move(yPadding, yPushDuration);
                }
            }

            _activeDialogPool.Enqueue(obj);
        }

        public void Disable()
        {
            if (_activeDialogPool.Count <= 0) return;

            DialogObject obj = _activeDialogPool.Dequeue();
            
            obj.Disable(() => {
                ValueTween.Stop(obj);
                obj.GetComponent<RectTransform>().position
                    = dialogLocation.position;
            });
        }

        public void DisableAll(bool fifs)
        {
            if (_activeDialogPool.Count <= 0) return;

            DialogObject obj = _activeDialogPool.Dequeue();
            
            obj.Disable(() => {
                ValueTween.Stop(obj);
                obj.GetComponent<RectTransform>().position
                    = dialogLocation.position;

                if (fifs)
                    DisableAll(fifs);
            });

            if (!fifs)
                DisableAll(fifs);
        }
    }
}