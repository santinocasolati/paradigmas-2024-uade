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

        public void Reset()
        {
            player = null;
            characterList.Clear();
            GameManager.Instance.Reset();
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
}
