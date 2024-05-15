﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum LevelType
    {
        Menu,
        Gameplay,
        Win,
        Lose
    }

    public class LevelManager
    {
        private static LevelManager instance = new LevelManager();

        private Level currentLevel = null;

        public LevelManager()
        {
            SetLevel(LevelType.Gameplay);
        }

        public static LevelManager Instance
        {
            get
            {
                return instance;
            }
        }

        public Level CurrentLevel
        {
            get
            {
                return currentLevel;
            }
        }

        public void SetLevel(LevelType levelType)
        {
            switch (levelType)
            {
                case LevelType.Menu:
                    break;
                case LevelType.Gameplay:
                    currentLevel = new GameplayLevel();
                    break;
                case LevelType.Win:
                    break;
                case LevelType.Lose:
                    break;
                default:
                    break;
            }
        }
    }
}
