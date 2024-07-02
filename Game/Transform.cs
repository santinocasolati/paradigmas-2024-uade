using System;
namespace Game
{
    public class Transform
    {
        public Vector2 position = new Vector2();
        public float rotation;
        public Vector2 scale = new Vector2();

        public void Move(Vector2 movement)
        {
            position.Add(movement);
        }
    }
}
