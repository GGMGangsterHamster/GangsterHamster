using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Objects.StageObjects
{
   public class ButtonActivatedChecker : MonoBehaviour, IEventable
   {
      public List<GameObject> Buttons = new List<GameObject>();
      private List<IActivated> _buttons = new List<IActivated>();

      public UnityEvent<GameObject> OnQualified;
      public UnityEvent<GameObject> OnDisqualified;

      private void Start()
      {
         for (int i = 0; i < Buttons.Count; ++i)
         {
            ICallbacks interactable = Buttons[i].GetComponent<ICallbacks>();

            if (interactable != null)
            {
               Event @event =
                  (interactable.Callbacks)
                  .Find(e => e.key == "");
               
               @event.OnActive.AddListener(Active);
               @event.OnDeactive.AddListener(Deactive);
               _buttons.Add(interactable);
            }
         }
      }

      public void Active(GameObject other)
      {
         if (_buttons.Find(e => !e.Activated) != null) return;

         OnQualified?.Invoke(this.gameObject);
      }

      public void Deactive(GameObject other)
      {
         if (_buttons.FindAll(e => !e.Activated).Count <= 0) return;

         Debug.Log("B");

         OnDisqualified?.Invoke(this.gameObject);
      }
   }
}