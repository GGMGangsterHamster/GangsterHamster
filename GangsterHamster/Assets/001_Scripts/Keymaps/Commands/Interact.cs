using Objects.Interactable.Management;
using Objects.UI.Dialog;

namespace Commands.Interaction
{
    class Interact : Command
    {
        public override void Execute()
        {
            InteractionManager.Instance.Interact();
        }
    }

    class NextDialog : Command
    {
        public override void Execute()
        {
            DialogManager.Instance.Next();
        }
    }
}