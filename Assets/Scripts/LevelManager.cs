using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    
    [SerializeField] private LevelData[] levels;
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private int maxLevel;
    public AnimationCurve difficultyCurve;

    private void Awake()
    {
        maxLevel = levels.Length;
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += InitializeLevel;

        GameManager.Instance.OnNextLevel += NextLevel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= InitializeLevel;

        GameManager.Instance.OnNextLevel -= NextLevel;    
    }

    
    private void InitializeLevel()
    {
        currentLevelIndex = DataManager.Instance.currentLevel-1; //currentLevel represents corresponding index at start

        
        var difficulty = GetDifficultyMultiplier();

        currentLevelIndex %= levels.Length;
        var levelData = levels[currentLevelIndex];
        //currentLevel = levelData.levelID;// increasing currentLevel
        
        //DataManager.Instance.currentLevel = currentLevelIndex + 1;
        
        EnemySpawner.Instance.StartSpawning(levelData,difficulty);
    }
    
    private float GetDifficultyMultiplier()
    {
        // Normalize the current level to a 0-1 range.
        float normalizedLevel = Mathf.Clamp01((float)(currentLevelIndex) / (maxLevel - 1));
        float multiplier = difficultyCurve.Evaluate(normalizedLevel);
        return multiplier;
    }
    
    private void NextLevel()
    {
        
    }
}
