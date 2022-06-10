using UnityEngine;
using System;
using System.Linq.Expressions;

namespace GridSystem
{
    /// <summary>
    /// Class meant to be used as an enum
    /// Contains useful classes to convert to or from Vectors
    /// </summary>
    public class Direction
    {
        private string value;
        public string Value => value;

        public const string Up = "Up";
        public const string Left = "Left";
        public const string Right = "Right";
        public const string Down = "Down";

        private Direction(string value) => this.value = value;

        public Direction Inverse() => value switch
        {
            Direction.Up => new Direction(Direction.Down),
            Direction.Left => new Direction(Direction.Right),
            Direction.Down => new Direction(Direction.Up),
            Direction.Right => new Direction(Direction.Left),
            _ => null
        };

        #region From Vector 2s

        public static Direction FromVector2Int(Vector2Int vec) => FromVector2(vec);

        public static Direction FromVector2(Vector2 vec)
        {
            if (vec == Vector2.zero)
            {
                // throw new Exception("Vector2 can't be null");
                return null;
            }

            float xAbs = Mathf.Abs(vec.x);
            float yAbs = Mathf.Abs(vec.y);

            if (xAbs >= yAbs)
            {
                return new Direction(vec.x > 0 ? Direction.Right : Direction.Left);
            }
            else
            {
                return new Direction(vec.y > 0 ? Direction.Up : Direction.Down);
            }
        }

        #endregion

        #region To Vector 2s

        Vector2Int ToVector2Int() => ToVector2Int(value);

        public static Vector2Int ToVector2Int(string direction) => direction switch
        {
            Direction.Up => Vector2Int.up,
            Direction.Left => Vector2Int.left,
            Direction.Down => Vector2Int.down,
            Direction.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
        public static Vector2 ToVector2(string direction) => ToVector2Int(direction);
        public static Vector3 ToVector3(string direction) => (Vector3) ToVector2(direction);
        #endregion

        public override string ToString() => value;
    }
}