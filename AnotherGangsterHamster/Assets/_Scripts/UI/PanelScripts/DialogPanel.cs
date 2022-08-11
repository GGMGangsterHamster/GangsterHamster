using UnityEngine;
using TMPro;
using Objects.Trigger;

namespace UI.PanelScripts
{
    public class DialogPanel : MonoBehaviour
    {
        private TMP_Text _dialogText;

        private void OnEnable()
        {
            if (_dialogText == null)
                _dialogText = GetComponentInChildren<TMP_Text>();

            _dialogText.text = "";
        }

        public void Show(string text)
        {
            _dialogText.text = text;

            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}