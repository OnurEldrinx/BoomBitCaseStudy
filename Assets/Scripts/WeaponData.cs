using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponData",fileName = "DefaultEnemy",order = 0)]
public class WeaponData : ScriptableObject
{
    public int fireRate;
    public int damage;
    public float range;
    public Bullet bulletPrefab;
    public LayerMask targetLayer;

}
