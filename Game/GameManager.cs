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
        private ObjectPool<Laser> _laserPool = new ObjectPool<Laser>(() => CharacterFactory.CreateCharacter(CharacterFactory.Characters.Laser) as Laser);

        public Action OnGameLost;
        public Action OnGameWin;

        public ObjectPool<Timer> TimerPool
        {
            get { return _timerPool; }
        }

        public ObjectPool<Laser> LaserPool
        {
            get { return _laserPool; }
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

        public void DestroyItem(Character t) 
        {
            if (t as Timer != null)
            {
                _timerPool.ReleaseObject(t as Timer);
            } else
            {
                _laserPool.ReleaseObject(t as Laser);
            }

            GameplayLevel gp = LevelManager.Instance.CurrentLevel as GameplayLevel;

            if (gp != null) 
            {
                gp.ItemDestroy(t);
            }
        }
    }
}
