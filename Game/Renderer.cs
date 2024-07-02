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

        public Renderer()
        {
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
                    Engine.Draw(_texture, transform.position.X, transform.position.Y, transform.scale.X, transform.scale.Y, transform.rotation, GetWidth * transform.scale.X / 2, GetHeight * transform.scale.Y / 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error trying to draw texture");
                }
            } else
            {
                Engine.Draw(_currentAnimation.CurrentFrame, transform.position.X, transform.position.Y, transform.scale.X, transform.scale.Y, transform.rotation, GetHeight * transform.scale.Y / 2);
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
