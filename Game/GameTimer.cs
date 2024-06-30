using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameTimer : IRenderer, IHasAnimations
    {
        private float currentTime;
        private float maxTime;

        private Animation clockAnim;
        private Animation handAnim;

        private float scale = .125f;
        private float handRotation = 0;

        private int RealWidthClock
        {
            get { return (int)(clockAnim.CurrentFrame.Width * scale); }
        }
        private int RealHeightClock
        {
            get { return (int)(clockAnim.CurrentFrame.Height * scale); }
        }

        private int RealWidthHand
        {
            get { return (int)(handAnim.CurrentFrame.Width * scale); }
        }
        private int RealHeightHand
        {
            get { return (int)(handAnim.CurrentFrame.Height * scale); }
        }

        public GameTimer(float currentTime, float maxTime)
        {
            this.currentTime = currentTime;
            this.maxTime = maxTime;

            AddAnimations();
        }

        public float CurrentTime { get { return currentTime; } }

        public void AddAnimations()
        {
            List<Texture> listClock = new List<Texture>();
            listClock.Add(Engine.GetTexture("Textures/Clock/clock.png"));
            clockAnim = new Animation("clock", listClock, 1, false);

            List<Texture> listHand = new List<Texture>();
            listHand.Add(Engine.GetTexture("Textures/Clock/clock_hand.png"));
            handAnim = new Animation("clockHand", listHand, 1, false);
        }

        public void AddTime(float timeToAdd)
        {
            currentTime += timeToAdd;
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
        }

        private void InterpolateTime()
        {
            handRotation = (currentTime / maxTime) * 360;
        }

        public void Draw()
        {
            Engine.Draw(clockAnim.CurrentFrame, 50, 50, scale, scale, 0, RealWidthClock / 2, RealHeightClock / 2);
            Engine.Draw(handAnim.CurrentFrame, 50, 50, scale, scale, handRotation, RealWidthHand / 2, RealHeightHand / 2);
        }
    }
}
