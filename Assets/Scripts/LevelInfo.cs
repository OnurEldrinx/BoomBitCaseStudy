using System;

[Serializable]
public class LevelInfo
{
    public int levelNumber;
    public int defeatedEnemyCount;

    public LevelInfo(int levelNumber, int defeatedEnemyCount)
    {
        this.levelNumber = levelNumber;
        this.defeatedEnemyCount = defeatedEnemyCount;
    }
}