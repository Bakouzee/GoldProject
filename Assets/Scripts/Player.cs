using GoldProject.Rooms;

namespace GoldProject
{
    public class Player : Entity
    {
        private CameraController cameraController;

        private void Start()
        {
            cameraController = FindObjectOfType<CameraController>();
            
            currentRoom = RoomsManager.Instance.FirstRoom;
            OnEnterRoom();
        }

        protected override void OnEnterRoom()
        {
            cameraController.ZoomToRoom(currentRoom);
        }
    }
}