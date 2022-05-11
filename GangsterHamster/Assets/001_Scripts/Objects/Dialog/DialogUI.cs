using UnityEngine;
using UnityEngine.UI;

namespace Objects.UI
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] GameObject _dialogPannel;
        [SerializeField] Text _text;
        [SerializeField] Image _icon;

        private void Awake()
        {
            _dialogPannel.SetActive(false);
        }

        /// <summary>
        /// 다이얼로그를 띄웁니다,
        /// </summary>
        /// <param name="message">보여줄 메세지</param>
        /// <param name="icon">아직은 사용되지 않음</param>
        public void Show(string message, Image icon)
        {
            _text.text = message;
            // _icon = icon;
            _dialogPannel.SetActive(true);
        }

        /// <summary>
        /// 다이얼로그를 닫습니다.
        /// </summary>
        public void Close()
        {
            _dialogPannel.SetActive(false);
        }
    }
}