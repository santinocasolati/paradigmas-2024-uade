﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Level
    {
        public abstract void Input();
        public abstract void Render();
    }

    public abstract class UpdatableLevel : Level
    {
        public abstract void Update(float deltaTime);
        protected abstract void CreateLevel();
    }

    public class GameplayLevel : UpdatableLevel
    {
        private GameUpdater gameUpdater;

        public GameplayLevel()
        {
            CreateLevel();
        }

        protected override void CreateLevel()
        {
            gameUpdater = new GameUpdater();

            Player p = new Player(new Vector2(.5f, .5f), new Vector2(Program.WIDTH / 2, Program.HEIGHT / 2), 500, 500);
            gameUpdater.AddPlayer(p);

            for (int i = 0; i < 10; i++)
            {
                Timer t = new Timer(new Vector2(1, 1), new Vector2(200, Program.HEIGHT / 2), 5, 7);
                gameUpdater.AddUpdatableObj(t);
            }
        }

        public override void Input()
        {
            gameUpdater.Input();
        }

        public override void Render()
        {
            Engine.Draw("Textures/Backgrounds/stars_bg.jpg", 0, 0, .25f, .25f, 0, 0, 0);
            gameUpdater.Render();
        }

        public override void Update(float deltaTime)
        {
            gameUpdater.Update(deltaTime);
        }
    }

    public class MenuLevel : Level
    {
        public override void Input()
        {
            if (Engine.GetKey(Keys.SPACE))
            {
                LevelManager.Instance.SetLevel(LevelType.Gameplay);
            }
        }

        public override void Render()
        {
            Engine.Draw("Textures/Backgrounds/main_menu.png", 0, 0, 1, 1, 0, 0, 0);
        }
    }

    public class WinLevel : Level
    {
        public override void Input()
        {
            if (Engine.GetKey(Keys.SPACE))
            {
                LevelManager.Instance.SetLevel(LevelType.Menu);
            }
        }

        public override void Render()
        {
            Engine.Draw("Textures/Backgrounds/win.png", 0, 0, 1, 1, 0, 0, 0);
        }
    }

    public class GameOverLevel : Level
    {
        public override void Input()
        {
            if (Engine.GetKey(Keys.SPACE))
            {
                LevelManager.Instance.SetLevel(LevelType.Menu);
            }
        }

        public override void Render()
        {
            Engine.Draw("Textures/Backgrounds/game_over.png", 0, 0, 1, 1, 0, 0, 0);
        }
    }
}
