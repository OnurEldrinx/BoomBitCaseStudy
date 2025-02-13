using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{
    
    private static readonly string FileName = "BoomBitCaseStudyGame.json";
    private static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

    
    public static void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true); 
        File.WriteAllText(FilePath, json);
        print("Game saved to: " + FilePath);
    }
    
    public static GameData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);
        
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(path);
            
            GameData data = JsonUtility.FromJson<GameData>(json);
            
            return data;
        }

        return new GameData(1,new List<LevelInfo>());
    }
}
