﻿using Microsoft.VisualBasic;
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
            player.Draw();

            foreach (var character in characterList)
            {
                character.Draw();
            }
        }

        private void CheckCollisions()
        {
            for (int i = 0; i < characterList.Count; i++)
            {
                Character other = characterList[i];
                if (IsBoxColliding(player.Position, player.RealSize,
                    other.Position, other.RealSize))
                {
                    player.Collide(other.Type);
                    other.Collide(player.Type);
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

        private Random random;

        private CharacterAnim redShip;
        private CharacterAnim greenShip;
        private CharacterAnim blueShip;

        public Player(Vector2 size, Vector2 position, float speed, float rotationSpeed) : base(size, position)
        {
            this.rotationSpeed = rotationSpeed;
            this.speed = speed;

            random = new Random();

            AddAnimations();

            GameUpdateManager.Instance.AddPlayer(this);
        }

        private void AddAnimations()
        {
            List<Texture> list = new List<Texture>();

            list.Add(Engine.GetTexture("Textures/Player/ship_red.png"));
            redShip = new CharacterAnim(CharacterType.Red, new Animation("redShip", list, .25f, true));

            list.Clear();
            list.Add(Engine.GetTexture("Textures/Player/ship_green.png"));
            greenShip = new CharacterAnim(CharacterType.Green, new Animation("greenShip", list, .25f, true));

            list.Clear();
            list.Add(Engine.GetTexture("Textures/Player/ship_blue.png"));
            blueShip = new CharacterAnim(CharacterType.Blue, new Animation("blueShip", list, .25f, true));

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

            int randomIndex = random.Next(0, tempList.Count);
            return tempList[randomIndex];
        }

        public override void Collide(CharacterType otherType)
        {
            base.Collide(otherType);

            if (otherType == characterType)
            {
                Console.WriteLine("Player Collided Current Color");

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

    public class Program
    {
        public static float deltaTime = 0;
        static DateTime lastFrameTime = DateTime.Now;

        public static int WIDTH = 800;
        public static int HEIGHT = 600;

        static void Main(string[] args)
        {
            Engine.Initialize("My Game" , WIDTH, HEIGHT);

            Player p = new Player(new Vector2(1, 1), new Vector2(WIDTH / 2, HEIGHT / 2), 5, 5);

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