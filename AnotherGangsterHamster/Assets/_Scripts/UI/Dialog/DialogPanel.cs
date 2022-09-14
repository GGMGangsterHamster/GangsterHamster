using System.Collections.Generic;
using UnityEngine;
using Tween;

namespace UI.Dialog
{
    public class DialogPanel : MonoBehaviour
    {
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

            obj.Enable(text);

            if (_activeDialogPool.Count > 0)
            {
                DialogObject[] arr = _activeDialogPool.ToArray();

                for (int i = 0; i < arr.Length; ++i)
                {
                    RectTransform trm
                        = arr[i].GetComponent<RectTransform>();

                    Vector3 pos = trm.position;
                    Vector3 target = trm.position;
                    target.y -= yPadding;
                    float step = yPushDuration / yPadding;

                    ValueTween.To(arr[i], () => {
                        pos.y -= step * Time.deltaTime;
                        trm.position = pos;
                        Debug.Log(trm.position);
                    }, () => trm.position.y <= target.y, () => {
                        trm.position = target;
                    });
                }
            }

            _activeDialogPool.Enqueue(obj);
        }

        public void Disable()
        {
            if (_activeDialogPool.Count <= 0) return;

            DialogObject obj = _activeDialogPool.Dequeue();
            obj.Disable(() => ValueTween.Stop(obj));
        }
    }
}