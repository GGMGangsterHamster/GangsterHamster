using Objects.Interactable.Management;

namespace Commands.Interaction
{
    class Interact : Command
    {
        public override void Execute()
        {
            InteractionManager.Instance.Interact();
        }
    }
}