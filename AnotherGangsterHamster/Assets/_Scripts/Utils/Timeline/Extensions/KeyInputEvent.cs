using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Timeline.Extensions
{    
    public class KeyInputEvent : MonoBehaviour, Objects.IActivated
    {
        public bool Activated => _activated;
        private bool _activated = false;

        public List<KeyCode> requireKeys
            = new List<KeyCode>();

        public bool stepByStep = true;

        private int _satisfiedCount = 0;

        private void OnEnable()
        {
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

            if (++_satisfiedCount < requireKeys.Count)
            {
                StartCoroutine(StepByStepInput());
            }
            else
                _activated = true;
        }

        IEnumerator InputChecker()
        {
            List<KeyCode> inputedKey = new List<KeyCode>();
            int inputCount = requireKeys.Count;

            while(!_activated)
            {
                yield return null;

                requireKeys.ForEach(key => {
                    if (Input.GetKey(key))
                    {
                        inputedKey.Add(key);
                        if (++_satisfiedCount >= inputCount)
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