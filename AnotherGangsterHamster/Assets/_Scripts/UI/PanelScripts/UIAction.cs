using System;
using UnityEngine;

namespace UI.PanelScripts
{
    public abstract class UIAction : MonoBehaviour, IUIAction
    {
        public abstract void ActivationActions();

        public abstract void DeActivationActions();

        public abstract void InitActions();

        public abstract void UpdateActions();

        public Action<float> soundAction;
        public Action<float> sensitivityAction;

        public int panelId;
        protected AudioSource _buttonClickSound;
    }

}