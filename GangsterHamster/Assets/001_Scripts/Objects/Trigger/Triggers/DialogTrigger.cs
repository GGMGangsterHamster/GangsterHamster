using UnityEngine;
using Objects.UI.Dialog;

namespace Objects.Trigger
{
    public class DialogTrigger : TriggerObject
    {
        [SerializeField] private string targetTag = "PLAYER_BASE";
        [SerializeField] private int dialogID = -1;

        private void Awake()
        {
            Awake();
            OnTrigger += (obj) => {
                Debug.Log("A");
                if(!obj.CompareTag(targetTag)) return;
                DialogManager.Instance.Show(dialogID);
                gameObject.SetActive(false);
            };
        }
    }
}