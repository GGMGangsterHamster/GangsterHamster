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

      private void Awake()
      {
         for (int i = 0; i < Buttons.Count; ++i)
         {
            _buttons.Add(Buttons[i].GetComponent<IActivated>());
         }
      }

      public void Active(GameObject other)
      {
         if (_buttons.Find(e => !e.Activated) != null) return;

         OnQualified?.Invoke(this.gameObject);
      }

      public void Deactive(GameObject other)
      {
         if (_buttons.Find(e => !e.Activated) != null) return;

         OnDisqualified?.Invoke(this.gameObject);
      }
   }
}