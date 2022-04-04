using UnityEngine;

namespace Characters.Player.Mouse
{
   [RequireComponent(typeof(Mouse))]
   public class MouseInputManager : MonoBehaviour
   {
      private MouseX _mouseX;
      private MouseY _mouseY;

      private Mouse _mouseMove = null;

      private void Start()
      {
         _mouseMove = GetComponent<Mouse>();

#if UNITY_EDITOR
         NULL.Check(_mouseMove, () =>
         {
            this.enabled = false;
         });
#endif

         _mouseX = new MouseX(_mouseMove);  // 마우스 X
         _mouseY = new MouseY(_mouseMove);  // 마우스 Y
      }

      private void Update()
      {
         _mouseX.Execute();
         _mouseY.Execute();
      }
   }
}