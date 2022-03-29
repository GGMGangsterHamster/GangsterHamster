using UnityEngine;
using Stage.Management;

public class SceneReloadTest : MonoBehaviour
{
   private void Update()
   {
      if(Input.GetKeyDown(KeyCode.Y))
         StageManager.Instance.Reload();
   }
}