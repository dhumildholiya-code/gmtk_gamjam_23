using UnityEngine;

namespace gmtk_gamejam.PropSystem
{
    public class DirectionalProp : BaseProp
    {
        public Direction direction;

        private void Start()
        {
            transform.rotation = GetRotation(direction);
        }

        public static Quaternion GetRotation(Direction direction)
        {
            Quaternion rotation = Quaternion.identity;
            switch (direction)
            {
                case Direction.North:
                    rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case Direction.South:
                    rotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case Direction.West:
                    rotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                case Direction.East:
                    rotation = Quaternion.Euler(0f, 0f, 270f);
                    break;
            }
            return rotation;
        }
    }
}
