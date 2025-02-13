using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelData",fileName = "DefaultLevel",order = 0)]
public class LevelData : ScriptableObject
{
    public int levelID;
    public EnemyData enemyType;
    public SerializedDictionary<int,int> waveCountTable;
    public float timeBetweenWaves;


    
    

}
