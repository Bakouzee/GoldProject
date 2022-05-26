using UnityEngine;
using System;

namespace GridSystem
{
    public class Direction
    {
        private string value;
        public string Value => value;
        
        public const string Up = "Up";
        public const string Left = "Left";
        public const string Right = "Right";
        public const string Down = "Down";

        private Direction(string value) => this.value = value;

        #region From Vector 2s
        public static Direction FromVector2Int(Vector2Int vec)
        {
            string dir = "";
            if (vec.x > 0)
                dir = Right;
            else if (vec.x < 0)
                dir = Left;
            else if (vec.y > 0)
                dir = Up;
            else if (vec.y < 0)
                dir = Down;
            
            return new Direction(dir);
        }
        public static Direction FromVector2(Vector2 vec)
        {
            if (vec == Vector2.zero)
                throw new Exception("Vector2 can't be null");

            Vector2[] vector2s = {Vector2.up, Vector2.left, Vector2.down, Vector2.right};
            string[] directions = {Direction.Up, Direction.Left, Direction.Down, Direction.Right};
            int closestDirectionIndex = 0;
            float closestDirectionAngle = Vector2.Angle(vec, vector2s[0]);
            for (int i = 1; i < vector2s.Length; i++)
            {
                float angle = Vector2.Angle(vec, vector2s[i]);
                if (angle < closestDirectionAngle)
                {
                    closestDirectionAngle = angle;
                    closestDirectionIndex = i;
                }
            }

            return new Direction(directions[closestDirectionIndex]);
        }
        #endregion

        #region To Vector 2s
        Vector2Int ToVector2Int() => ToVector2Int(value);
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
        #endregion

        public override string ToString() => value;
    }
}
