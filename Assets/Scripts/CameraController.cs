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
            if (room.isCorridor)
            {
                // Focus all map
                size = roomsManager.mapSize;
                wantedPos = size * 0.5f;
            }
            else
            {
                // Focus only room
                wantedPos = room.Position;
                size = room.size;
            }
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