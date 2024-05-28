using System;
namespace Game
{
    public static class PickableFactory
    {
        public enum Pickable { Timer };

        public static IPickable CreatePickableObject(Pickable pickable)
        {
            switch(pickable)
            {
                case Pickable.Timer:
                    return new Timer(new Vector2(1, 1), new Vector2(200, Program.HEIGHT / 2), 5, 7);

                default:
                    break;
            }

            return null;
        }
    }
}
