using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameTimer : IRenderer
    {
        private float currentTime;
        private float maxTime;

        private float scale = .125f;

        private Transform clockTransform;
        private Renderer clockRenderer;

        private Transform handTransform;
        private Renderer handRenderer;

        public GameTimer(float currentTime, float maxTime)
        {
            this.currentTime = currentTime;
            this.maxTime = maxTime;

            clockTransform = new Transform();
            clockTransform.position = new Vector2(50, 50);
            clockTransform.scale.X = scale;
            clockTransform.scale.Y = scale;
            clockRenderer = new Renderer(Engine.GetTexture("Textures/Clock/clock.png"));

            handTransform = new Transform();
            handTransform.position = new Vector2(50, 50);
            handTransform.scale.X = scale;
            handTransform.scale.Y = scale;
            handRenderer = new Renderer(Engine.GetTexture("Textures/Clock/clock_hand.png"));
        }

        public float CurrentTime { get { return currentTime; } }

        public void AddTime(float timeToAdd)
        {
            currentTime += timeToAdd;

            if (currentTime >= maxTime)
            {
                GameManager.Instance.OnGameWin?.Invoke();
            }
        }

        public void SetTime(float currentTime, float maxTime)
        {
            this.currentTime = currentTime;
            this.maxTime = maxTime;
        }

        public void RemoveTime(float timeToRemove)
        {
            currentTime -= timeToRemove;
        }

        public void Update(float deltaTime)
        {
            currentTime -= deltaTime;
            InterpolateTime();

            if (currentTime <= 0)
            {
                GameManager.Instance.OnGameLost?.Invoke();
            }
        }

        private void InterpolateTime()
        {
            handTransform.rotation = (currentTime / maxTime) * 360;
        }

        public void Draw()
        {
            clockRenderer.Draw(clockTransform);
            handRenderer.Draw(handTransform);
        }
    }
}
