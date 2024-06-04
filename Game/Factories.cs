using System;
namespace Game
{
    public static class CharacterFactory
    {
        public enum Characters { Timer };

        public static ICharacter CreateCharacter(Characters character)
        {
            switch(character)
            {
                case Characters.Timer:
                    return new Timer(new Vector2(1, 1), new Vector2(200, Program.HEIGHT / 2), 5, 7);

                default:
                    break;
            }

            return null;
        }
    }
}
