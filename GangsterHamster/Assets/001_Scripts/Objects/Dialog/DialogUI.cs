using UnityEngine;
using UnityEngine.UI;

namespace Objects.UI
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] Text _text;
        [SerializeField] Image _icon;

        public void Show(string message, Image icon)
        {
            _text.text = message;
            _icon = icon;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}