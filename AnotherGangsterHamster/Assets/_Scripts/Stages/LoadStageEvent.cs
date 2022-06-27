using Objects;
using Stages.Management;
using UnityEngine;

namespace Stages
{
   public class LoadStageEvent : MonoBehaviour, ICollisionEventable
   {
      public new string name;

      public void Active(GameObject other)
      {
         StageManager.Instance.Load(name);
      }

      public void Deactive(GameObject other) { }
   }
}