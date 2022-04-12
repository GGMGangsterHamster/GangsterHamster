using UnityEngine;

namespace UI.PanelScripts
{
    public class InGameAction : UIAction
    {

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
        }
    }
}
