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

    public class Character : ICollidable, IRenderer, IHasAnimations, ICharacter
    {
        protected Transform transform;
        protected Renderer renderer;
        protected CharacterType characterType;

        public Character(Transform transform, Texture texture)
        {
            this.transform = transform;
            renderer = new Renderer(texture);
        }

        public Character(Transform transform, Animation currentAnimation)
        {
            this.transform = transform;
            renderer = new Renderer(currentAnimation);
        }

        public Character(Transform transform)
        {
            this.transform = transform;
            renderer = new Renderer();
        }

        public int PosX
        {
            get { return (int)transform.position.X; }
        }
        public int PosY
        {
            get { return (int)transform.position.Y; }
        }

        protected int RealWidth
        {
            get { return (int)(renderer.GetWidth * transform.scale.X); }
        }
        protected int RealHeight
        {
            get { return (int)(renderer.GetHeight * transform.scale.Y); }
        }

        public Vector2 Position
        {
            get { return transform.position; }
        }

        public Vector2 Size
        {
            get { return transform.scale; }
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
            renderer.Draw(transform);
        }

        public virtual void Update(float deltaTime)
        {
            renderer.Update();
        }
    }

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

        public Player(Transform transform, float speed, float rotationSpeed) : base(transform)
        {
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
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

            if (renderer == null)
            {
                renderer = new Renderer(selectedAnim.Anim);
            } else
            {
                renderer.ChangeAnimation(selectedAnim.Anim);
            }

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
            defaultUpVector.X = 0;
            defaultUpVector.Y = 1;

            float cosTheta = (float)Math.Cos(rotationRadians);
            float sinTheta = (float)Math.Sin(rotationRadians);

            Vector2 currentUpVector = new Vector2();
            currentUpVector.X = defaultUpVector.X * cosTheta + defaultUpVector.Y * sinTheta;
            currentUpVector.Y = defaultUpVector.X * sinTheta - defaultUpVector.Y * cosTheta;
            return currentUpVector;
        }

        public void Input()
        {
            float newRotation = transform.rotation;

            if (Engine.GetKey(Keys.A))
            {
                newRotation -= rotationSpeed * Program.deltaTime;
            }

            if (Engine.GetKey(Keys.D))
            {
                newRotation += rotationSpeed * Program.deltaTime;
            }

            transform.rotation = newRotation;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            CheckBorders();
            AddSpeed(deltaTime);
        }

        private void AddSpeed(float deltaTime)
        {
            Vector2 upVector = CalculateUpVector(transform.rotation);
            upVector.ScaleVector(speed);
            upVector.ScaleVector(deltaTime);
            transform.position.X += upVector.X;
            transform.position.Y += upVector.Y;
        }

        private void CheckBorders()
        {
            if (transform.position.X < 0)
            {
                transform.position.X = Program.WIDTH;
            }
            else if (transform.position.X > Program.WIDTH)
            {
                transform.position.X = 0;
            }

            if (transform.position.Y < 0)
            {
                transform.position.Y = Program.HEIGHT;
            }
            else if (transform.position.Y > Program.HEIGHT)
            {
                transform.position.Y = 0;
            }
        }
    }

    public class Timer : Character, IPickable
    {
        private float lifeTime;
        private int minLifeTime;
        private int maxLifeTime;
        private float currentTime;

        private CharacterAnim redTimer;
        private CharacterAnim greenTimer;
        private CharacterAnim blueTimer;

        public Timer(Transform transform, int minLifeTime, int maxLifeTime) : base(transform)
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
        }

        public void Reset()
        {
            CharacterAnim selectedAnim = SelectRandomAnimation();
            renderer.ChangeAnimation(selectedAnim.Anim);
            characterType = selectedAnim.Type;
            transform.position.X = Program.random.Next(0, Program.WIDTH);
            transform.position.Y = Program.random.Next(0, Program.HEIGHT);
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

            if (otherType == characterType)
            {
                Pick();
            }
            else
            {
                GameManager.Instance.RemoveTime(5);
            }

            GameManager.Instance.DestroyTimer(this);
        }

        public void Pick()
        {
            GameManager.Instance.AddTime(5);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            currentTime += deltaTime;

            if (currentTime >= lifeTime)
            {
                GameManager.Instance.DestroyTimer(this);
            }
        }
    }
}
