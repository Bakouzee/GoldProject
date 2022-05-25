using UnityEngine;

namespace GridSystem
{
    public class Direction
    {
        private string value;
        
        public const string Up = "Up";
        public const string Left = "Left";
        public const string Right = "Right";
        public const string Down = "Down";

        public Vector2 ToVector2()
        {
            switch (value)
            {
                case Direction.Up:
                    return Vector2.up;
                case Direction.Down:
                    return Vector2.down;
                case Direction.Left:
                    return Vector2.left;
                case Direction.Right:
                    return Vector2.right;
                default:
                    return Vector2.zero;
            }
        }

        public Vector3 ToVector3()
        {
            return (Vector3)ToVector2();
        }
    }
}