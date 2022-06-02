using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects
{
    public class InteractionDelay : MonoBehaviour
    {
        public float delay;

        public List<UnityEvent<GameObject>> OnActiveEvent;
        public List<UnityEvent<GameObject>> OnDeactiveEvent;

        private GameObject obj;

        public void OnActive(GameObject obj)
        {
            this.obj = obj;
            StartCoroutine(DelayCoroutine(true, obj));
            
        }

        public void OnDeactive(GameObject obj)
        {
            this.obj = obj;
            StartCoroutine(DelayCoroutine(false, obj));
        }

        IEnumerator DelayCoroutine(bool isActivate, GameObject obj)
        {
            yield return new WaitForSeconds(delay);

            foreach(UnityEvent<GameObject> evt in (isActivate ? OnActiveEvent : OnDeactiveEvent))
            {
                evt?.Invoke(obj);
            }
        }
    }
}