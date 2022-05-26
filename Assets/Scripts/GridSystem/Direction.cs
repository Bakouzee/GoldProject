using UnityEngine;
using System;

namespace GridSystem
{
    public class Direction
    {
        public string value;
        
        public const string Up = "Up";
        public const string Left = "Left";
        public const string Right = "Right";
        public const string Down = "Down";

        public Direction(string _value) => value = _value;
        

        public static Vector2Int ToVector2Int(string direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector2Int.up;
                case Direction.Down:
                    return Vector2Int.down;
                case Direction.Left:
                    return Vector2Int.left;
                case Direction.Right:
                    return Vector2Int.right;
                default:
                    return Vector2Int.zero;
            }
        }
        public static Vector2 ToVector2(string direction)
        {
            switch (direction)
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

        public static Vector3 ToVector3(string direction)
        {
            return (Vector3)ToVector2(direction);
        }
    }
}