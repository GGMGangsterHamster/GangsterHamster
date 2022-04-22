using UnityEngine;

namespace Characters.Player.Mouse
{
   [RequireComponent(typeof(Mouse))]
   public class MouseInputManager : MonoBehaviour
   {

      // IMouseDeltaRecvable 구체화 한 클레스
      private Mouse _mouseMove = null;
      
      // 마우스 커멘드
      private MouseX _mouseX;
      private MouseY _mouseY;

      private void Start()
      {
         _mouseMove = GetComponent<Mouse>();

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