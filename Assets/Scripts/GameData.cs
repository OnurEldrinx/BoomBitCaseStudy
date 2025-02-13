using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int currentLevel;
    public List<LevelInfo> levelsInfoList;

    public GameData(int currentLevel, List<LevelInfo> levelsInfoList)
    {
        this.currentLevel = currentLevel;
        this.levelsInfoList = levelsInfoList;
    }
}
