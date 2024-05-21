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
    }

    public abstract class UpdatableLevel : Level
    {
        public abstract void Update(float deltaTime);
        protected abstract void CreateLevel();
    }

    public class GameplayLevel : UpdatableLevel
    {
        private Player player;
        private List<Character> characterList = new List<Character>();

        public GameplayLevel()
        {
            CreateLevel();
        }

        protected override void CreateLevel()
        {
            player = new Player(new Vector2(.5f, .5f), new Vector2(Program.WIDTH / 2, Program.HEIGHT / 2), 500, 500);

            for (int i = 0; i < 10; i++)
            {
                Timer t = new Timer(new Vector2(1, 1), new Vector2(200, Program.HEIGHT / 2), 5, 7);
                characterList.Add(t);
            }
        }

        public override void Input()
        {
            player?.Input();
        }

        public override void Render()
        {
            Engine.Draw("Textures/Backgrounds/stars_bg.jpg", 0, 0, .25f, .25f, 0, 0, 0);
            GameManager.Instance.DrawTimer();

            foreach (Character character in characterList)
            {
                character.Draw();
            }

            player?.Draw();
        }

        public override void Update(float deltaTime)
        {
            player?.Update(deltaTime);

            foreach (Character character in characterList)
            {
                character.Update(deltaTime);
            }

            CheckCollisions();

            GameManager.Instance.UpdateTimer(deltaTime);
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
