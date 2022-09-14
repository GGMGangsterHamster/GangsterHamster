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

            if (obj == null)
                obj = Add();

            obj.Enable(text);

            if (_activeDialogPool.Count > 0)
            {
                DialogObject[] arr = _activeDialogPool.ToArray();

                for (int i = 0; i < arr.Length; ++i)
                {
                    RectTransform trm
                        = arr[i].GetComponent<RectTransform>();

                    Vector2 pos    = trm.anchoredPosition;
                    Vector2 target = trm.anchoredPosition;
                    target.y -= yPadding;

                    float defaultYPos = trm.anchoredPosition.y;
                    float step = yPushDuration / (HALF_PI / yPadding);
                    float t = 0.0f;

                    ValueTween.To(arr[i], () => {
                        t += step * Time.deltaTime;
                        pos.y = defaultYPos - Mathf.Sin(t) * yPadding;
                        trm.anchoredPosition = pos;
                        Debug.Log(trm.anchoredPosition);
                    }, () => t >= HALF_PI, () => {
                        trm.anchoredPosition = target;
                    });
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
    }
}