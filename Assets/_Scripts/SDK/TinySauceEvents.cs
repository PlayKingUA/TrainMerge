using System.Collections;
using System.Collections.Generic;
using _Scripts.Game_States;
using _Scripts.Levels;
using _Scripts.Units;
using UnityEngine;
using Zenject;

public class TinySauceEvents : MonoBehaviour
{
    #region Variables
        
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private ZombieManager _zombieManager;

        private int _playTime;
        private int _levelNumber;
        private string _levelName;
        // amount of start events
        private int _levelCount;
        private int _levelLoop;
        private const string LevelCountKey = "LevelCountKey";
        
        #endregion
        
        private void Start()
        {
            Load();
            _gameStateManager.AttackStarted += LevelStartEvent;
            _gameStateManager.Victory += () =>
            {
                LevelFinishEvent(true);
            };
            _gameStateManager.Fail += () =>
            {
                LevelFinishEvent(false);
            };
        }

        private void LevelStartEvent()
        {
            _playTime = (int) Time.time;

            _levelNumber = _levelManager.LevelNumber;
            _levelName = "level_" + _levelManager.CurrentLevel;
            _levelLoop = _levelNumber / _levelManager.CurrentLevel + 1;
            IncreaseLevelCount();
        
            var eventParameters = "{\"level_number\":\"" + _levelNumber + "\"," +
                                  "\"level_name\":\"" + _levelName + "\"," +
                                  "\"level_count\":\"" + _levelCount + "\"," +
                                  "\"level_loop\":\"" + _levelLoop + "\"," +
                                  "\"level_random\":\"" + 0 + "\"," +
                                  "\"level_type\":\"normal\"," +
                                  "}";

            TinySauce.OnGameStarted(_levelNumber.ToString());
            
            /*AppMetrica.Instance.ReportEvent("level_start", eventParameters);
            AppMetrica.Instance.SendEventsBuffer();*/
        }
        
        private void LevelFinishEvent(bool isWin)
        {
            var levelResult = isWin ? "win" : "lose";
            _playTime = (int) (Time.time - _playTime);
        
            var eventParameters = "{\"level_number\":\"" + _levelNumber + "\"," +
                                  "\"level_name\":\"" + _levelName + "\"," +
                                  "\"level_count\":\"" + _levelCount + "\"," +
                                  "\"level_loop\":\"" + _levelLoop + "\"," +
                                  "\"level_random\":\"" + 0 + "\"," +
                                  "\"level_type\":\"normal\"," +
                                  "\"result\":\"" + levelResult +"\"," +
                                  "\"time\":\"" + _playTime +"\"," +
                                  "\"progress\":\"" + ((int) (_zombieManager.Progress * 100)) +"\"," +
                                  "\"continue\":\"0\"," +
                                  "}";

            TinySauce.OnGameFinished(isWin, _zombieManager.Progress,"UserGameLevelNumber");
            
            /*AppMetrica.Instance.ReportEvent("level_finish", eventParameters);
            AppMetrica.Instance.SendEventsBuffer();*/
        }
        
        private void IncreaseLevelCount()
        {
            _levelCount++;
            Save();
        }

        #region Save/Load
        private void Load()
        {
            _levelCount = PlayerPrefs.GetInt(LevelCountKey);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(LevelCountKey, _levelCount);
        }
        #endregion
}
