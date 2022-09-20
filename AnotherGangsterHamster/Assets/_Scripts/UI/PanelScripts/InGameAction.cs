using Objects.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PanelScripts
{
    public class InGameAction : UIAction
    {
        private Transform _mainCameraTransform;

        public Image aimImage;
        public Transform canGrapUI;

        [Header("에임포인트 색깔")]
        public List<AimColor> aimColors = new List<AimColor>();

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
                Utils.StopTime();
                UIManager.Instance.ActivationPanel(UIPanels.Pause);
            }

            if (InteractionManager.Instance.currentRaycastHitTrm != null)
            {
                foreach(AimColor ac in aimColors)
                {
                    if(InteractionManager.Instance.currentRaycastHitTrm.CompareTag(ac.tag))
                    {
                        aimImage.color = ac.tagColor;
                    }
                }
            }
            else
            {
                aimImage.color = aimColors[0].tagColor;
            }

            canGrapUI.gameObject.SetActive(InteractionManager.Instance.CanGrep);
        }

        
    }
}
