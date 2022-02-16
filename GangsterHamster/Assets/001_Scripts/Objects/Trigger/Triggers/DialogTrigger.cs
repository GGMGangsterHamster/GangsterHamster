using UnityEngine;
using Objects.UI.Dialog;

namespace Objects.Trigger
{
    public class DialogTrigger : TriggerObject
    {
        [SerializeField] private string targetTag = "PLAYER";
        [SerializeField] private int dialogID = -1;

        protected override void Awake()
        {
            base.Awake();
            OnTrigger += (obj) => {
                Debug.Log("A");
                if(!obj.CompareTag(targetTag)) return;
                DialogManager.Instance.Show(dialogID);
                gameObject.SetActive(false);
            };
        }
    }
}