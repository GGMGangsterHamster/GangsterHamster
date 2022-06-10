using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
   public class DisplayText : MonoBehaviour
   {
      [SerializeField] private Text _displayText;
      [SerializeField] private Text _singleChar;

      private string _lastText = "";
      private string _lastChar = "";

      private void Awake()
      {
         _displayText.text = "";
         _singleChar.text = "";
      }

      public void Display(string text)
      {
         _displayText.text = text;
      }

      public void Append(string text)
      {
         _displayText.text += text;
      }

      public void DisplaySingleChar(string ch)
      {
         _singleChar.text = ch;
      }

      public void ClearLine()
      {
         _displayText.text = "";
      }

      public void ClearChar()
      {
         _singleChar.text = "";
      }
   }
}