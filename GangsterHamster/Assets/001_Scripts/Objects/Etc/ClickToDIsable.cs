using UnityEngine;

public class ClickToDIsable : MonoBehaviour
{


   public GameObject one;
   public GameObject two;

   public void Click()
   {

      if (two.activeSelf) {
         Cursor.lockState = CursorLockMode.Locked;
         Time.timeScale = 1.0f;
         this.gameObject.SetActive(false);
      } else {
            one.SetActive(false);
            two.SetActive(true);
      }
   }
}