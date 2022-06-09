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
      public float closeAfter = 5.0f;

      public void Active(GameObject other)
      {
         DialogManager.Instance.DisplayDialog(dialogType, dialogId);

         Invoke(nameof(Deactive), closeAfter);
      }

      public void Deactive(GameObject other)
      {
         DialogManager.Instance.ClearDialog();
      }

      public void Deactive()
      {
         Deactive(null);
      }
   }
}