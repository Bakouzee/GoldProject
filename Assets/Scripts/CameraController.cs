using System;
using GoldProject.Rooms;
using UnityEngine;

namespace GoldProject
{
    public class CameraController : MonoBehaviour
    {
        private float zPos;
        private Camera _camera;
        public Camera Camera => _camera;
        public bool dezoomCam;
        private Resolution resolution;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            zPos = transform.position.z;
            resolution = Screen.currentResolution;
        }

        public void ZoomToRoom(Room room)
        {
            RoomsManager roomsManager = RoomsManager.Instance;
            Vector2 wantedPos;
            Vector2 size;

            if(transform.parent != null)
            {
                transform.parent = null;
            }

            /*if (room.isCorridor)
            {
                // Focus all map
                size = roomsManager.mapSize;
                wantedPos = size * 0.5f;

                // Focus the player
                transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                size = roomsManager.playerFovSize;
                wantedPos = transform.parent.position;
            }
            else
            {*/
                switch (room.name)
                {
                    case "Salle d'armes":
                        wantedPos = new Vector2(room.Position.x + 2, room.Position.y + 2);
                        break;
                    case "Salon":
                        wantedPos = new Vector2(room.Position.x - 2, room.Position.y + 2);
                        break;
                    case "Chambre":
                        wantedPos = new Vector2(room.Position.x - 2, room.Position.y - 2);
                        break;
                    case "Bibliothéque":
                        wantedPos = new Vector2(room.Position.x + 2, room.Position.y - 2);
                        break;
                    case "Couloir Ouest":
                        wantedPos = new Vector2(room.Position.x, room.Position.y);
                        break;
                    case "Couloir Est":
                        wantedPos = new Vector2(room.Position.x, room.Position.y);
                        break;
                    case "Couloir Nord":
                        wantedPos = new Vector2(room.Position.x, room.Position.y);
                        break;
                    default:
                        wantedPos = new Vector2(room.Position.x, room.Position.y);
                        break;
                }
                // Focus only room
                size = new Vector2Int(room.size.x + 5, room.size.y + 5);
            //}
            transform.position = new Vector3(wantedPos.x, wantedPos.y, zPos);
            _camera.orthographicSize = GetOrthographicSizeDesired(size);
        }

        private float GetOrthographicSizeDesired(Vector2 size)
        {
            float widthByHeight = (float) resolution.width / (float) resolution.height;
            float heightByWidth = (float) resolution.height / (float) resolution.width;
            if (size.y >= size.x * heightByWidth)
            {
                // Room is vertical
                return size.y * 0.5f;
            }
            else
            {
                // Room is horizontal
                return size.x * 0.5f * heightByWidth;
            }
        }
    }
}