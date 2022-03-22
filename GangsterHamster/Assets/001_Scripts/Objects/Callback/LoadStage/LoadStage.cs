using Stage.Management;
using Stage;
using UnityEngine;

namespace Objects.Callback
{
   public class LoadStage : MonoBehaviour, ICallbackable
   {
      [SerializeField] StageNames target;

      public void Invoke(object param)
      {
         StageManager.Instance.Load(target);
         Destroy(this.gameObject);
      }
   }
}