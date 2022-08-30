using UnityEngine;
using TMPro;
using Objects.Trigger;

namespace UI.PanelScripts
{
    public class DialogPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _dialogText;
        [SerializeField] private TMP_Text _continueText;

        public string defaultContiueText = "Press \'E\' to continue";

        public void Show(string text)
        {
            _dialogText.text = text;
            _continueText.text = defaultContiueText;

            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}