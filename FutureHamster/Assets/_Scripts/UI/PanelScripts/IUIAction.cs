using UnityEngine;

namespace UI.PanelScripts
{
    interface IUIAction
    {
        /// <summary>
        /// 처음 게임이 시작 되었을 때 한번만 호출하는 함수
        /// </summary>
        public void InitActions();

        /// <summary>
        /// 해당하는 패널이 활성화 될때마다 호출하는 함수
        /// </summary>
        public void ActivationActions();

        /// <summary>
        /// 해당하는 패널이 비활성화 될때마다 호출하는 함수
        /// </summary>
        public void DeActivationActions();

        /// <summary>
        /// 패널이 활성화 되어 있을 때 
        /// 프레임마다 호출하는 함수ㄴ
        /// </summary>
        public void UpdateActions();
    }

}