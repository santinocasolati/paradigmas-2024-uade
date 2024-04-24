using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Game
{
   
    public class GameUpdateManager
    {
        private static GameUpdateManager instance = new GameUpdateManager();
        public static GameUpdateManager Instance { get { return instance; } }

        private Player player;
        private List<Character> characterList = new List<Character>();

        public GameUpdateManager() { }

        public void AddPlayer(Player player)
        {
            this.player = player;
        }

        public void AddUpdatableObj(Character character)
        {
            characterList.Add(character);
        }

        public void RemoveUpdatableObj(Character character)
        {
            characterList.Remove(character);
        }

        public void Input()
        { 
            player.Input();
        }

        public void Update() 
        {
            player.Update();

            foreach (var character in characterList)
            {
                character.Update();
            }

            CheckCollisions();
        }

        public void Render()
        {
            foreach (var character in characterList)
            {
                character.Draw();
            }

            player.Draw();
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                Character other = characterList[i];
                if (IsBoxColliding(player.Position, player.RealSize,
                    other.Position, other.RealSize))
                {
                    CharacterType playerType = player.Type;
                    CharacterType otherType = other.Type;

                    player.Collide(otherType);
                    other.Collide(playerType);
                }
            }
        }

        private bool IsBoxColliding(int FirstPosX, int FirstPosY,
         int FirstRealWidth, int FirstRealHeight, int ScndPosX, int ScndPosY,
         int ScndRealWidth, int ScndRealHeight)
        {

            float distanceX = Math.Abs(FirstPosX - ScndPosX);
            float distanceY = Math.Abs(FirstPosY - ScndPosY);

            float sumHalfWidths = FirstRealWidth / 2 + ScndRealWidth / 2;
            float sumHalfHeights = FirstRealHeight / 2 + ScndRealHeight / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }

        private bool IsBoxColliding(Vector2 posOne, Vector2 realSizeOne,
        Vector2 posTwo, Vector2 RealSizeTwo)
        {

            float distanceX = Math.Abs(posOne.x - posTwo.x);
            float distanceY = Math.Abs(posOne.y - posTwo.y);

            float sumHalfWidths = realSizeOne.x / 2 + RealSizeTwo.x / 2;
            float sumHalfHeights = realSizeOne.y / 2 + RealSizeTwo.y / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }

    }

    public enum CharacterType
    {
        Red,
        Green,
        Blue,
        Obstacle
    }

    public class Character
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

        public virtual void Collide(CharacterType otherType) { }

        public virtual void Draw()
        {
            Engine.Draw(currentAnimation.CurrentFrame, position.x, position.y, size.x, size.y, rotation, RealWidth / 2, RealHeight / 2);
        }

        public virtual void Input() { }

        public virtual void Update()
        {
            currentAnimation.Update();
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

    public class Player : Character
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

            GameUpdateManager.Instance.AddPlayer(this);
        }

        private void AddAnimations()
        {
            List<Texture> listRed = new List<Texture>();
            listRed.Add(Engine.GetTexture("Textures/Player/ship_red.png"));
            redShip = new CharacterAnim(CharacterType.Red, new Animation("redShip", listRed, .25f, true));

            List<Texture> listGreen = new List<Texture>();
            listGreen.Add(Engine.GetTexture("Textures/Player/ship_green.png"));
            greenShip = new CharacterAnim(CharacterType.Green, new Animation("greenShip", listGreen, .25f, true));

            List<Texture> listBlue = new List<Texture>();
            listBlue.Add(Engine.GetTexture("Textures/Player/ship_blue.png"));
            blueShip = new CharacterAnim(CharacterType.Blue, new Animation("blueShip", listBlue, .25f, true));

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
                ChangeColor();
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

        public override void Input()
        {
            base.Input();

            float newRotation = rotation;

            if (Engine.GetKey(Keys.A))
            {
                newRotation -= rotationSpeed;
            }

            if (Engine.GetKey(Keys.D))
            {
                newRotation += rotationSpeed;
            }

            rotation = newRotation;
        }

        public override void Update()
        {
            base.Update();

            CheckBorders();
            AddSpeed();
        }

        private void AddSpeed()
        {
            Vector2 upVector = CalculateUpVector(rotation);
            upVector.ScaleVector(speed);
            position.x += upVector.x;
            position.y += upVector.y;
        }

        private void CheckBorders()
        {
            if (position.x < 0)
            {
                position.x = Program.WIDTH;
            } else if (position.x > Program.WIDTH)
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

            GameUpdateManager.Instance.AddUpdatableObj(this);
        }

        private void AddAnimations()
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

            if (otherType == characterType)
            {
                Generate();
            }
        }

        public override void Update()
        {
            base.Update();
            currentTime += Program.deltaTime;

            if (currentTime >= lifeTime)
            {
                Generate();
            }
        }
    }

    public class Program
    {
        public static float deltaTime = 0;
        static DateTime lastFrameTime = DateTime.Now;

        public static int WIDTH = 800;
        public static int HEIGHT = 600;

        public static Random random = new Random();

        static void Main(string[] args)
        {
            Engine.Initialize("My Game" , WIDTH, HEIGHT);

            Player p = new Player(new Vector2(1, 1), new Vector2(WIDTH / 2, HEIGHT / 2), 5, 5);

            for (int i = 0; i < 10; i++)
            {
                Timer t = new Timer(new Vector2(1, 1), new Vector2(200, HEIGHT / 2), 5, 7);
            }

            while (true)
            {   
                Input();
                Update();
                Render();

                CalcDeltaTime();
            }
        }

        private static void CalcDeltaTime()
        {
            TimeSpan deltaSpan = DateTime.Now - lastFrameTime;
            deltaTime = (float)deltaSpan.TotalSeconds;
            lastFrameTime = DateTime.Now;
        }

        static void Input()
        {
            GameUpdateManager.Instance.Input();
        }

        static void Update()
        {
            GameUpdateManager.Instance.Update();
        }

        static void Render()
        {
            Engine.Clear();

            GameUpdateManager.Instance.Render();

            Engine.Show();
        }
    }
}