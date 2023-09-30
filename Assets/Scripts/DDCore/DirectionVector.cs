using UnityEngine;

namespace DDCore
{
    public static class DirectionVector
    {

        public static Vector2Int Convert(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Vector2Int.left;
                case Direction.Right:
                    return Vector2Int.right;
                case Direction.Up:
                    return Vector2Int.up;
                case Direction.Down:
                    return Vector2Int.down;
                default:
                    DebugWrapper.LogError($"{direction} does not have an implementation for conversion!");
                    return Vector2Int.zero;
            }
        }

        public static Direction Convert(Vector2Int vector)
        {
            if (vector == Vector2Int.left)
            {
                return Direction.Left;
            }
            else if (vector == Vector2Int.right)
            {
                return Direction.Right;
            }
            else if (vector == Vector2Int.up)
            {
                return Direction.Up;
            }
            else if (vector == Vector2Int.down)
            {
                return Direction.Down;
            }
            else
            {
                DebugWrapper.LogError($"{vector} does not match any of the preset directions, returning Left.");
                return Direction.Left;
            }
        }
    }
}
