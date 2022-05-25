using Unity.Notifications.Android;
using UnityEngine;

namespace GridSystem
{
    public abstract class GridController : MonoBehaviour
    {
        public void Move(Direction direction)
        {
            Vector2 dir = direction.ToVector2();
            
            // Move
            transform.position += (Vector3)dir;
            
            // Rotate in move direction
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}