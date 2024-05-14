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

        private GameTimer gameTimer;
        private bool gameInProgress = false;

        public GameUpdateManager()
        {
            gameTimer = new GameTimer(10);
            gameInProgress = true;
        }

        public void AddTime(float timeToAdd)
        {
            gameTimer.AddTime(timeToAdd);
        }

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
            if (!gameInProgress) return;

            player?.Input();
        }

        public void Update() 
        {
            if (!gameInProgress) return;

            player?.Update();

            foreach (var character in characterList)
            {
                character.Update();
            }

            CheckCollisions();

            float timeLeft = gameTimer.UpdateTime(Program.deltaTime);

            if (timeLeft <= 0)
            {
                gameInProgress = false;
            }
        }

        public void Render()
        {
            if (!gameInProgress) return;

            gameTimer.Draw();

            foreach (var character in characterList)
            {
                character.Draw();
            }

            player?.Draw();
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

    public class GameTimer
    {
        private float currentTime;

        public GameTimer(float currentTime)
        {
            this.currentTime = currentTime;
        }

        public void AddTime(float timeToAdd)
        {
            currentTime += timeToAdd;
        }

        public float UpdateTime(float deltaTime)
        {
            currentTime -= deltaTime;
            return currentTime;
        }

        public void Draw()
        {
            Console.WriteLine((int)currentTime);
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

            Player p = new Player(new Vector2(1, 1), new Vector2(WIDTH / 2, HEIGHT / 2), 500, 500);

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