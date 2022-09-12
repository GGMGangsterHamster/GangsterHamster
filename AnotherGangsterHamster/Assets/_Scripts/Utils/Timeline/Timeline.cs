using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timeline
{
    public class @Timeline : MonoBehaviour
    {
        public List<TimedEvent> timedEvents
            = new List<TimedEvent>();

        public List<ExecuteAfterEvent> executeAfterEvents
            = new List<ExecuteAfterEvent>();

        public List<ExecuteOnSatisfiedEvent> executeOnSatisfiedEvents
            = new List<ExecuteOnSatisfiedEvent>();

        private void Start()
        {
            timedEvents.ForEach(@event => {
                StartCoroutine(TimedEventLoop(@event));
            });

            executeOnSatisfiedEvents.ForEach(@event => {
                @event.Init();
                StartCoroutine(SatisfiedEventLoop(@event));
            });
        }


        IEnumerator TimedEventLoop(TimedEvent @event)
        {
            yield return new WaitForSeconds(@event.delay);

            @event.actions.Invoke();
            ExecuteFollowingEvents(@event.key);
        }

        IEnumerator SatisfiedEventLoop(ExecuteOnSatisfiedEvent @event)
        {
            yield return new WaitUntil(() => @event.satisfied);

            @event.actions.Invoke();
            ExecuteFollowingEvents(@event.key);
        }


        private void ExecuteFollowingEvents(string key)
        {
            executeAfterEvents
                .FindAll(x => x.eventBefore.CompareTo(key) == 0)
                ?.ForEach(x => {
                    StartCoroutine(TimedEventLoop(x));
                });
        }
    }
}