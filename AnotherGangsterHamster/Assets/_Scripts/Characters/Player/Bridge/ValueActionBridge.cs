using Sound;
using UnityEngine;
using UI.PanelScripts;

namespace Characters.Player.Bridge
{
   public class ValueActionBridge
   {
      public ValueActionBridge()
      {
         UIManager.Instance.soundAction += volume => {
            SoundManager.Instance.GlobalVolume = volume;
            BackgroundMusic.Instance.SetVolume(volume);
            Debug.Log("Set volume to " + volume);
         };

         UIManager.Instance.sensitivityAction += sensitivity => {
            PlayerValues.MouseSpeed = sensitivity;
            Debug.Log("Set sensitivity to " + sensitivity);
         };
      }
   }
}