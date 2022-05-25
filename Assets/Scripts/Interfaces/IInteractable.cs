namespace GoldProject
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        public void Interact();
    }
}