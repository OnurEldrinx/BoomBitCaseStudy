using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyDetector : MonoBehaviour
{
    
    [SerializeField] private float detectionRadius;
    [SerializeField] private List<Enemy> enemiesInRange;
    public List<Enemy> EnemiesInRange => enemiesInRange;

    private SphereCollider _sphereCollider;
    private WeaponSystem _weaponSystem;

    public static Action<Enemy> EnemyOutOfRange;
    
    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = detectionRadius;
        _sphereCollider.isTrigger = true;
        _weaponSystem = transform.root.GetComponentInChildren<WeaponSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy e) && !enemiesInRange.Contains(e))
        {
            enemiesInRange.Add(e);
            //_weaponSystem.ShootTarget.Invoke(e);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy e) && enemiesInRange.Contains(e))
        {
            enemiesInRange.Remove(e);
            EnemyOutOfRange.Invoke(e);
        }
    }
    

    public void CleanUpEnemies()
    {
        enemiesInRange.RemoveAll(e => !e.gameObject.activeInHierarchy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    public void SetRange(float r)
    {
        detectionRadius = r;
    }
    
}
