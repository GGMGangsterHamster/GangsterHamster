using UnityEngine;
using Stages.Management;
using Objects.InteractableObjects;

namespace Objects.Trigger
{
   [RequireComponent(typeof(TriggerInteractableObject))]
   public class ShowDialogTrigger : MonoBehaviour, IEventable
   {
      const string PLAYER = "PLAYER_BASE";

      public string dialogType = "tutroial";
      public int dialogId = 0;
      public float closeAfter = 5.0f;

      private int _key;

      public void Active(GameObject other)
      {
         DialogManager.Instance.DisplayDialog(dialogType, dialogId);
         _key = Authority.Instance.GetOwnership("DIALOG");

         Invoke(nameof(Deactive), closeAfter);
      }

      public void Deactive(GameObject other)
      {
         if (Authority.Instance.CheckOwnership("DIALOG", _key))
            DialogManager.Instance.ClearDialog();
      }

      public void Deactive()
      {
         Deactive(null);
      }
   }
}