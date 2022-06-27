using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.StageObjects
{
   public class ButtonActivatedChecker : MonoBehaviour, ICollisionEventable
   {
      public List<GameObject> Buttons = new List<GameObject>();
      private List<IActivated> _buttons = new List<IActivated>();

      public UnityEvent<GameObject> OnQualified;
      public UnityEvent<GameObject> OnDisqualified;

      private void Start()
      {
         for (int i = 0; i < Buttons.Count; ++i)
         {
            dynamic interactable;

            interactable = Buttons[i].GetComponent<CollisionInteractableObject>();
            if (interactable == null)
               interactable = Buttons[i].GetComponent<TriggerInteractableObject>();
            if (interactable == null)
               interactable = Buttons[i].GetComponent<BothInteractableObject>();

            if (interactable != null)
            {
               interactable._callbacks.Add(
                  new CollisionCallback(e => {
                     Active(this.gameObject);
                     Debug.Log("ASD");
                  }, e => {
                     Deactive(this.gameObject);
                     Debug.Log("ASSDDD");
                  })
               );
               _buttons.Add(Buttons[i].GetComponent<IActivated>());
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

         OnDisqualified?.Invoke(this.gameObject);
      }
   }
}