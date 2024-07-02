using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameManager
    {
        private static GameManager instance = new GameManager();

        private readonly float timeToWin = 100;
        private readonly float timeToLose = 0;
        private readonly float startingTime = 50;

        private GameTimer gameTimer;
        private ObjectPool<Timer> _timerPool = new ObjectPool<Timer>(() => CharacterFactory.CreateCharacter(CharacterFactory.Characters.Timer) as Timer);

        public Action OnGameLost;
        public Action OnGameWin;

        public ObjectPool<Timer> TimerPool
        {
            get { return _timerPool; }
        }

        public GameManager()
        {
            gameTimer = new GameTimer(startingTime, timeToWin);

            OnGameLost += HandleGameLost;
            OnGameWin += HandleGameWin;
        }

        private void HandleGameWin()
        {
            LevelManager.Instance.SetLevel(LevelType.Win);
        }

        private void HandleGameLost()
        {
            LevelManager.Instance.SetLevel(LevelType.Lose);
        }

        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void Reset()
        {
            gameTimer.SetTime(startingTime, timeToWin);
        }

        public void AddTime(float timeToAdd)
        {
            gameTimer.AddTime(timeToAdd);
        }

        public void RemoveTime(float timeToRemove)
        {
            gameTimer.RemoveTime(timeToRemove);
        }

        public void UpdateTimer(float deltaTime)
        {
            gameTimer.Update(deltaTime);
        }

        public void DrawTimer()
        {
            gameTimer.Draw();
        }

        public void DestroyTimer(Timer t) 
        {
            _timerPool.ReleaseObject(t);

            GameplayLevel gp = LevelManager.Instance.CurrentLevel as GameplayLevel;

            if (gp != null) 
            {
                gp.TimerDestroy(t);
            }
        }
    }
}
