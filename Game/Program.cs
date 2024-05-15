using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Game
{
    public class Program
    {
        public static float deltaTime = 0;
        static DateTime lastFrameTime = DateTime.Now;

        public static int WIDTH = 1200;
        public static int HEIGHT = 800;

        public static Random random = new Random();

        static void Main(string[] args)
        {
            Engine.Initialize("My Game" , WIDTH, HEIGHT);

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
            LevelManager.Instance.CurrentLevel?.Input();
        }

        static void Update()
        {
            LevelManager.Instance.CurrentLevel?.Update(deltaTime);
        }

        static void Render()
        {
            Engine.Clear();

            LevelManager.Instance.CurrentLevel?.Render();

            Engine.Show();
        }
    }
}