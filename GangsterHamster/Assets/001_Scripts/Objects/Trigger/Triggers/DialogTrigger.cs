using UnityEngine;
using Objects.UI.Dialog;

namespace Objects.Trigger
{
    public class DialogTrigger : TriggerObject
    {
        [SerializeField] private int dialogID = -1;

        protected override void Awake()
        {
            base.Awake();
            OnTrigger += (obj) => {
                DialogManager.Instance.Show(dialogID);
                gameObject.SetActive(false);
            };
        }
    }
}