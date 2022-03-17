using UnityEngine;
using Utils.Camera;

public class CameraSwitchTest : MonoBehaviour
{
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         CameraChanger.Instance.EnableCamera("Main");
      }
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         CameraChanger.Instance.EnableCamera("Sub1");
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
         CameraChanger.Instance.EnableCamera("Sub2");
      }
   }
}