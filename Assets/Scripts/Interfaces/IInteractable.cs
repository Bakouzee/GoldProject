namespace GoldProject
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        public bool NeedToBeInRange { get; }
        public void Interact();
    }
}