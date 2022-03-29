using Commands.Movement.Mouse;
using Player.Mouse;

namespace Player.Movement
{
    public class MouseInputManager : MonoSingleton<MouseInputManager>
    {
        private MouseX _mouseX;
        private MouseY _mouseY;

        private MouseMovement _mouseMove = null;

        private void Start()
        {
            _mouseMove = FindObjectOfType<MouseMovement>();

#if UNITY_EDITOR
            NULL.Check(_mouseMove, () => {
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