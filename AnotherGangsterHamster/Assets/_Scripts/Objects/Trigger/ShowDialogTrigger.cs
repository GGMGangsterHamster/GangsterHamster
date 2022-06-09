using UnityEngine;
using Stages.Management;

namespace Objects.Trigger
{
   [RequireComponent(typeof(TriggerInteractableObject))]
   public class ShowDialogTrigger : MonoBehaviour, ICollisionEventable
   {
      const string PLAYER = "PLAYER_BASE";

      public string dialogType = "tutroial";
      public int dialogId = 0;

      public void Active(GameObject other)
      {
         DialogManager.Instance.GetDialog(dialogType, dialogId);

         // TODO: Enable UI
      }

      public void Deactive(GameObject other)
      {
         // TODO: Disable UI
      }
   }
}