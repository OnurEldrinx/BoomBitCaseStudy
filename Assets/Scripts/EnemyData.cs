using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyData",fileName = "DefaultEnemy",order = 0)]
public class EnemyData : ScriptableObject
{
    public int health;
    public int damage;
    public float pathUpdateInterval;
    
    

}
