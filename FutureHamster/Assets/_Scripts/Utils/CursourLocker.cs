using UnityEngine;

static partial class Utils
{
   static public void LockCursor()
   {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
   }

   static public void UnlockCursor()
   {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
   }
}