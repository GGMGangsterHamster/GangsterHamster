using UnityEngine;

namespace Characters.Player.Mouse
{
   [RequireComponent(typeof(Mouse))]
   public class MouseInputManager : MonoBehaviour
   {
      // IMouseDeltaRecvable 구체화 한 클레스
      private Mouse _mouseMove = null;
      
      // 마우스 커멘드
      public MouseX mouseX;
      public MouseY mouseY;

      private void Awake()
      {
         _mouseMove = GetComponent<Mouse>();

         mouseX = new MouseX(_mouseMove);  // 마우스 X
         mouseY = new MouseY(_mouseMove);  // 마우스 Y
      }

      private void Update()
      {
         mouseX.Execute?.Invoke(null);
         mouseY.Execute?.Invoke(null);
      }
   }
}