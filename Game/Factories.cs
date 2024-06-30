﻿using System;
namespace Game
{
    public static class CharacterFactory
    {
        public enum Characters { Timer, Player };

        public static ICharacter CreateCharacter(Characters character)
        {
            switch (character)
            {
                case Characters.Timer:
                    Transform timerTransform = new Transform();
                    timerTransform.scale = new Vector2(1, 1);
                    timerTransform.position = new Vector2(200, Program.HEIGHT / 2);
                    return new Timer(timerTransform, 5, 7);

                case Characters.Player:
                    Transform playerTransform = new Transform();
                    playerTransform.scale = new Vector2(.5f, .5f);
                    playerTransform.position = new Vector2(Program.WIDTH / 2, Program.HEIGHT / 2);
                    return new Player(playerTransform, 500, 500);

                default:
                    break;
            }

            return null;
        }
    }
}
