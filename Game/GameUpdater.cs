using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameUpdater
    {
        private Player player;
        private List<Character> characterList = new List<Character>();

        public void SetPlayer(Player player)
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
            player?.Input();
        }

        public void Update(float deltaTime)
        {
            player?.Update(deltaTime);

            foreach (Character character in characterList)
            {
                character.Update(deltaTime);
            }

            CheckCollisions();

            GameManager.Instance.UpdateTimer(deltaTime);
        }

        public void Render()
        {
            GameManager.Instance.DrawTimer();

            foreach (Character character in characterList)
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

        private bool IsBoxColliding(Vector2 posOne, Vector2 realSizeOne,
        Vector2 posTwo, Vector2 RealSizeTwo)
        {

            float distanceX = Math.Abs(posOne.X - posTwo.X);
            float distanceY = Math.Abs(posOne.Y - posTwo.Y);

            float sumHalfWidths = realSizeOne.X / 2 + RealSizeTwo.X / 2;
            float sumHalfHeights = realSizeOne.Y / 2 + RealSizeTwo.Y / 2;

            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeights;
        }

    }
}
