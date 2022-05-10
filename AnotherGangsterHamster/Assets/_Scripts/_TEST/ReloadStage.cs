using UnityEngine;
using Stages.Management;

public class ReloadStage : MonoBehaviour
{
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.H))
      {
         // StageManager.Instance.Load(StageNames.Stage_1);
      }

   }
}