using System;
using System.Collections.Generic;

public class DataManager : Singleton<DataManager>
{
    public int currentLevel;
    private int _defeatedEnemyCount;
    public int TotalEnemyCount { get; set; }

    public GameData GameData { get; private set; }


    private void Awake()
    {
        
        GameData = DataSerializer.LoadGame();
        currentLevel = GameData.currentLevel;
        
        if (GameData is null)
        {
            return;
        }

        print("Current Level: " + GameData.currentLevel);
        print("Enemies Defeated: " + GameData.levelsInfoList.Find(l=>l.levelNumber == currentLevel)?.defeatedEnemyCount);
        
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += () =>
        {
            TotalEnemyCount = 0;
            _defeatedEnemyCount = 0;
        };

        GameManager.Instance.OnNextLevel += () =>
        {
            SaveGame();
        };

    }
    
    public void SaveGame()
    {
        // Update gameData as needed before saving
        GameData.currentLevel = currentLevel + 1;
        GameData.levelsInfoList.Add(new LevelInfo(currentLevel,_defeatedEnemyCount));
        DataSerializer.SaveGame(GameData);
        _defeatedEnemyCount = 0;
        currentLevel++;
    }
    

    public void UpdateDefeatedEnemiesTable(int value)
    {
        _defeatedEnemyCount+=value;
        UIManager.Instance.UpdateKillCounterText(GetEnemyCount());

        if (_defeatedEnemyCount == TotalEnemyCount)
        {
            GameManager.Instance.SetState(GameState.Win);
        }

        
    }
    
    public int GetEnemyCount()
    {
        return _defeatedEnemyCount;
    }
    
}
