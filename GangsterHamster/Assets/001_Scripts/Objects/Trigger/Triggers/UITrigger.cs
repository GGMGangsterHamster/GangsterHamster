using UnityEngine;
using Objects.UI.Dialog;
using Objects.Callback;

namespace Objects.Trigger
{
   public class UITrigger : TriggerObject, ICallbackable
   {
      [SerializeField] private string targetTag = "PLAYER_BASE";
      public GameObject cvs = null;

      protected override void Awake()
      {
         base.Awake();
         OnTrigger += (obj) =>
         {
            Foo(obj);
         };
      }

      public void Invoke(object param)
      {
         Foo(null, true);
      }

      void Foo(GameObject obj, bool force = false)
      {
         if (obj == null || !obj.CompareTag(targetTag)) {
            if(!force) return;
         }
         
         Cursor.lockState = CursorLockMode.None;
         cvs.SetActive(true);
         Time.timeScale = 0.0f;
         this.gameObject.SetActive(false);
      }

   }
}