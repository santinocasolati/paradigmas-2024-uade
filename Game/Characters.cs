using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum CharacterType
    {
        Red,
        Green,
        Blue,
        Obstacle
    }

    public class Character : ICollidable, IDrawable, IHasAnimations
    {
        protected Vector2 size;
        protected Vector2 position;
        protected float rotation;
        protected CharacterType characterType;

        protected Animation currentAnimation;

        public Character(Vector2 size, Vector2 position)
        {
            this.size = size;
            this.position = position;
        }

        public int PosX
        {
            get { return (int)position.x; }
        }
        public int PosY
        {
            get { return (int)position.y; }
        }

        protected int RealWidth
        {
            get { return (int)(currentAnimation.CurrentFrame.Width * size.x); }
        }
        protected int RealHeight
        {
            get { return (int)(currentAnimation.CurrentFrame.Height * size.y); }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Size
        {
            get { return size; }
        }
        public Vector2 RealSize
        {
            get { return new Vector2(RealWidth, RealHeight); }
        }

        public CharacterType Type
        {
            get { return characterType; }
        }

        public virtual void AddAnimations() { }

        public virtual void Collide(CharacterType otherType) { }

        public virtual void Draw()
        {
            Engine.Draw(currentAnimation.CurrentFrame, position.x, position.y, size.x, size.y, rotation, RealWidth / 2, RealHeight / 2);
        }

        public virtual void Update(float deltaTime)
        {
            currentAnimation.Update();
        }
    }

    //Composicion. Animation tambien es composicion
    public class CharacterAnim
    {
        private CharacterType characterType;
        private Animation animation;

        public CharacterAnim(CharacterType characterType, Animation animation)
        {
            this.characterType = characterType;
            this.animation = animation;
        }

        public CharacterType Type
        {
            get { return characterType; }
        }

        public Animation Anim
        {
            get { return animation; }
        }
    }

    public class Player : Character, IRecievesInput
    {
        private float speed;
        private float rotationSpeed;

        private CharacterAnim redShip;
        private CharacterAnim greenShip;
        private CharacterAnim blueShip;

        public Player(Vector2 size, Vector2 position, float speed, float rotationSpeed) : base(size, position)
        {
            this.rotationSpeed = rotationSpeed;
            this.speed = speed;

            AddAnimations();
        }

        public override void AddAnimations()
        {
            List<Texture> listRed = new List<Texture>();
            List<Texture> listGreen = new List<Texture>();
            List<Texture> listBlue = new List<Texture>();

            for (int i = 0; i < 6; i++)
            {
                listRed.Add(Engine.GetTexture($"Textures/Player/Red/0{i + 1}.png"));
                listGreen.Add(Engine.GetTexture($"Textures/Player/Green/0{i + 1}.png"));
                listBlue.Add(Engine.GetTexture($"Textures/Player/Blue/0{i + 1}.png"));
            }

            
            redShip = new CharacterAnim(CharacterType.Red, new Animation("redShip", listRed, .5f, true));
            greenShip = new CharacterAnim(CharacterType.Green, new Animation("greenShip", listGreen, .5f, true));
            blueShip = new CharacterAnim(CharacterType.Blue, new Animation("blueShip", listBlue, .5f, true));

            ChangeColor();
        }

        private void ChangeColor()
        {
            CharacterAnim selectedAnim = SelectRandomAnimation();
            currentAnimation = selectedAnim.Anim;
            characterType = selectedAnim.Type;
        }

        private CharacterAnim SelectRandomAnimation()
        {
            List<CharacterAnim> tempList = new List<CharacterAnim>();
            tempList.Add(redShip);
            tempList.Add(greenShip);
            tempList.Add(blueShip);

            int randomIndex = Program.random.Next(0, tempList.Count);
            return tempList[randomIndex];
        }

        public override void Collide(CharacterType otherType)
        {
            base.Collide(otherType);

            if (otherType == characterType)
            {
                CharacterType currentColor = characterType;

                while (currentColor == characterType)
                {
                    ChangeColor();
                }
            }

            if (otherType == CharacterType.Obstacle)
            {
                Console.WriteLine("Player Collided an Obstacle");
            }
        }

        private Vector2 CalculateUpVector(float rotation)
        {
            double rotationRadians = Math.PI * rotation / 180;

            Vector2 defaultUpVector = new Vector2();
            defaultUpVector.x = 0;
            defaultUpVector.y = 1;

            float cosTheta = (float)Math.Cos(rotationRadians);
            float sinTheta = (float)Math.Sin(rotationRadians);

            Vector2 currentUpVector = new Vector2();
            currentUpVector.x = defaultUpVector.x * cosTheta + defaultUpVector.y * sinTheta;
            currentUpVector.y = defaultUpVector.x * sinTheta - defaultUpVector.y * cosTheta;
            return currentUpVector;
        }

        public void Input()
        {
            float newRotation = rotation;

            if (Engine.GetKey(Keys.A))
            {
                newRotation -= rotationSpeed * Program.deltaTime;
            }

            if (Engine.GetKey(Keys.D))
            {
                newRotation += rotationSpeed * Program.deltaTime;
            }

            rotation = newRotation;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            CheckBorders();
            AddSpeed(deltaTime);
        }

        private void AddSpeed(float deltaTime)
        {
            Vector2 upVector = CalculateUpVector(rotation);
            upVector.ScaleVector(speed);
            upVector.ScaleVector(deltaTime);
            position.x += upVector.x;
            position.y += upVector.y;
        }

        private void CheckBorders()
        {
            if (position.x < 0)
            {
                position.x = Program.WIDTH;
            }
            else if (position.x > Program.WIDTH)
            {
                position.x = 0;
            }

            if (position.y < 0)
            {
                position.y = Program.HEIGHT;
            }
            else if (position.y > Program.HEIGHT)
            {
                position.y = 0;
            }
        }
    }

    public class Timer : Character
    {
        private float lifeTime;
        private int minLifeTime;
        private int maxLifeTime;
        private float currentTime;

        private CharacterAnim redTimer;
        private CharacterAnim greenTimer;
        private CharacterAnim blueTimer;

        public Timer(Vector2 size, Vector2 position, int minLifeTime, int maxLifeTime) : base(size, position)
        {
            this.minLifeTime = minLifeTime;
            this.maxLifeTime = maxLifeTime;

            AddAnimations();
        }

        public override void AddAnimations()
        {
            List<Texture> listRed = new List<Texture>();
            listRed.Add(Engine.GetTexture("Textures/Timer/timer_red.png"));
            redTimer = new CharacterAnim(CharacterType.Red, new Animation("redTimer", listRed, .25f, true));

            List<Texture> listGreen = new List<Texture>();
            listGreen.Add(Engine.GetTexture("Textures/Timer/timer_green.png"));
            greenTimer = new CharacterAnim(CharacterType.Green, new Animation("greenTimer", listGreen, .25f, true));

            List<Texture> listBlue = new List<Texture>();
            listBlue.Add(Engine.GetTexture("Textures/Timer/timer_blue.png"));
            blueTimer = new CharacterAnim(CharacterType.Blue, new Animation("blueTimer", listBlue, .25f, true));

            Generate();
        }

        private void Generate()
        {
            CharacterAnim selectedAnim = SelectRandomAnimation();
            currentAnimation = selectedAnim.Anim;
            characterType = selectedAnim.Type;
            position.x = Program.random.Next(0, Program.WIDTH);
            position.y = Program.random.Next(0, Program.HEIGHT);
            lifeTime = Program.random.Next(minLifeTime, maxLifeTime);
            currentTime = 0;
        }

        private CharacterAnim SelectRandomAnimation()
        {
            List<CharacterAnim> tempList = new List<CharacterAnim>();
            tempList.Add(redTimer);
            tempList.Add(greenTimer);
            tempList.Add(blueTimer);

            int randomIndex = Program.random.Next(0, tempList.Count);
            return tempList[randomIndex];
        }

        public override void Collide(CharacterType otherType)
        {
            base.Collide(otherType);

            GameplayLevel castedLevel = LevelManager.Instance.CurrentLevel as GameplayLevel;

            if (castedLevel != null )
            {
                if (otherType == characterType)
                {
                    castedLevel.AddTime(2);
                }
                else
                {
                    castedLevel.RemoveTime(2);
                }

                Generate();
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            currentTime += deltaTime;

            if (currentTime >= lifeTime)
            {
                Generate();
            }
        }
    }
}
