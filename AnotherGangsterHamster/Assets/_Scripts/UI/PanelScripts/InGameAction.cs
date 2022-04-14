using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class InGameAction : UIAction
    {
        private Transform _mainCameraTransform;

        public Image aimImage;

        [Header("에임포인트 색깔")]
        public Color ATYPEOBJ;
        public Color DEFAULT;

        private Transform MainCameraTransform
        {
            get
            {
                if(_mainCameraTransform == null)
                {
                    _mainCameraTransform = Camera.main.transform;
                }

                return _mainCameraTransform;
            }
        }

        public override void ActivationActions()
        {

        }

        public override void DeActivationActions()
        {

        }

        public override void InitActions()
        {
            panelId = 5;
        }

        public override void UpdateActions()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Utils.UnlockCursor();
                UIManager.Instance.ActivationPanel(UIPanels.Pause);
            }

            if (UnityEngine.Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out RaycastHit hit))
            {
                if (hit.transform.tag == "ATYPEOBJECT")
                {
                    aimImage.color = ATYPEOBJ;
                }
                else
                {
                    aimImage.color = DEFAULT;
                }
            }
        }
    }
}
