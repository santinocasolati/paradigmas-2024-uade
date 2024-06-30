using System;
namespace Game
{
    public class Renderer
    {
        private Animation _currentAnimation;
        private Texture _texture;

        public Renderer(Texture texture)
        {
            this._texture = texture;
        }

        public Renderer(Animation animation)
        {
            this._currentAnimation = animation;
        }

        public Renderer(Animation animation, Texture texture)
        {
            this._currentAnimation = animation;
            this._texture = texture;
        }

        public int GetWidth
        {
            get
            {
                if (_currentAnimation != null)
                {
                    return (int)_currentAnimation.CurrentFrame.Width;
                } else
                {
                    return (int)_texture.Width;
                }
            }
        }

        public int GetHeight
        {
            get
            {
                if(_currentAnimation != null)
                {
                    return (int)_currentAnimation.CurrentFrame.Height;
                } else
                {
                    return (int)_texture.Height;
                }
            }
        }

        public void ChangeAnimation(Animation animation)
        {
            _currentAnimation = animation;
        }

        public void Draw(Transform transform)
        {
            if (_currentAnimation == null)
            {
                try
                {
                    Engine.Draw(_texture, transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y, transform.Rotation.X);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error trying to draw texture");
                }
            } else
            {
                Engine.Draw(_currentAnimation.CurrentFrame, transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y, transform.Rotation.X);
            }
        }

        public void Update()
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.Update();
            }
        }
    }
}
