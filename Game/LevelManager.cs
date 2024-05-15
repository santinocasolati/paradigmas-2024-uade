using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class LevelManager
    {
        private static LevelManager instance = new LevelManager();

        private Dictionary<string, Level> levels = new Dictionary<string, Level>();
        private Level currentLevel = null;

        public LevelManager()
        {
            GameLevel gameLevel = new GameLevel();
            AddNewLevel("game", gameLevel);
            SetLevel("game");
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

        public void SetLevel(string levelName)
        {
            if (levels.TryGetValue(levelName, out var l_currentLevel))
            {
                l_currentLevel.Reset();
                currentLevel = l_currentLevel;
            } else
            {
                Console.WriteLine($"Level {levelName} has not been found");
            }
        }

        public void AddNewLevel(string levelName, Level level)
        {
            levels.Add(levelName, level);
        }
    }
}
