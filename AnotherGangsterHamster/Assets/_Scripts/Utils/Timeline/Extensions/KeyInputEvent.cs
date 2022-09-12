using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Timeline.Extensions
{    
    public class KeyInputEvent : MonoBehaviour, Objects.IActivated
    {
        [Header("NULL 인 경우 부모 Timeline 사용")]
        public Timeline connectedTimeline = null;

        [Header("연결할 이벤트 Key")]
        public string connectedEventKey = "";
        
        private ExecuteOnSatisfiedEvent _connectedEvent;

        public bool Activated => _activated;
        private bool _activated 
        {
            get => _act;
            set
            {
                _act = value;
                _connectedEvent.OnStatusChanged();
            }
        }
        private bool _act = false;


        public List<KeyCode> requireKeys
            = new List<KeyCode>();

        public bool stepByStep = true;

        private int _satisfiedCount = 0;
        private int _inputCount = 0;

        private void OnEnable()
        {
            if (connectedTimeline == null)
                connectedTimeline = transform.GetComponentInParent<Timeline>();

            Debug.Assert(connectedTimeline != null);
            
            _connectedEvent =
                connectedTimeline
                .executeOnSatisfiedEvents
                .Find(@event => @event.key == connectedEventKey);

            _inputCount = requireKeys.Count;


            if (stepByStep)
                StartCoroutine(StepByStepInput());
            else
                StartCoroutine(InputChecker());
        }


        IEnumerator StepByStepInput()
        {
            yield return new WaitUntil(() => {
                return Input.GetKey(requireKeys[_satisfiedCount]);
            });

            if (++_satisfiedCount < _inputCount)
            {
                StartCoroutine(StepByStepInput());
            }
            else
                _activated = true;
        }

        IEnumerator InputChecker()
        {
            List<KeyCode> inputedKey = new List<KeyCode>();

            while(!_activated)
            {
                yield return null;

                requireKeys.ForEach(key => {
                    if (Input.GetKey(key))
                    {
                        inputedKey.Add(key);
                        if (++_satisfiedCount >= _inputCount)
                            _activated = true;
                    }
                });

                inputedKey.ForEach(key =>{
                    requireKeys.Remove(key);
                });
            }
        }
    }
}