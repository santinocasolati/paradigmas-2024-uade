using System;
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
        public abstract void Update(float deltaTime);
        public abstract void Reset();

        protected abstract void CreateLevel();
    }

    public class GameLevel : Level
    {
        protected override void CreateLevel()
        {
            Player p = new Player(new Vector2(1, 1), new Vector2(Program.WIDTH / 2, Program.HEIGHT / 2), 500, 500);

            for (int i = 0; i < 10; i++)
            {
                Timer t = new Timer(new Vector2(1, 1), new Vector2(200, Program.HEIGHT / 2), 5, 7);
            }
        }

        public override void Input()
        {
            GameUpdateManager.Instance.Input();
        }

        public override void Render()
        {
            Engine.Draw("Textures/Backgrounds/stars_bg.jpg", 0, 0, .25f, .25f, 0, 0, 0);
            GameUpdateManager.Instance.Render();
        }

        public override void Reset()
        {
            GameUpdateManager.Instance.Reset();
            CreateLevel();
        }

        public override void Update(float deltaTime)
        {
            GameUpdateManager.Instance.Update(deltaTime);
        }
    }
}
