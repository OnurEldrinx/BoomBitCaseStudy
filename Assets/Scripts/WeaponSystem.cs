using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Settings")] 
    [SerializeField] private float fireRate; // shots per second
    
    [Header("References")]
    [SerializeField] private EnemyDetector enemyDetector;
    [SerializeField] private SpriteRenderer targetIndicator;
    [SerializeField] private WeaponData data;


    private float _nextFireTime;
    private PlayerMovement _playerMovement; 
    
    public Bullet bulletPrefab;
    public Transform muzzle;
    public VisualEffect muzzleFlash;
    
    [Header("Bullet Pool Settings")]
    [SerializeField] private int capacity = 50;
    [SerializeField] private int maxCapacity = 100;
    private IObjectPool<Bullet> _bulletPool;
    private readonly bool _poolCheck = true;

    private Enemy _target;
    
    private void Awake()
    {
        enemyDetector = transform.root.GetComponentInChildren<EnemyDetector>();
        _playerMovement = transform.root.GetComponentInChildren<PlayerMovement>();

        fireRate = data.fireRate;
        bulletPrefab = data.bulletPrefab;
        enemyDetector.SetRange(data.range);
        muzzleFlash.playRate = 1;

        
        _bulletPool = new ObjectPool<Bullet>(CreateBullet,OnGetBullet,OnReturnBullet,OnDestroyBullet,_poolCheck,capacity,maxCapacity);
        
    }

    private Bullet CreateBullet()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.Pool = _bulletPool;
        return bullet;
    }
    private void OnGetBullet(Bullet b)
    {
        b.gameObject.SetActive(true);
    }
    
    private void OnReturnBullet(Bullet b)
    {
        b.transform.parent = null;
        b.gameObject.SetActive(false);
    }
    
    private void OnDestroyBullet(Bullet b)
    {
        Destroy(b.gameObject);
    }
    
    private void Update()
    {
        if (Time.time >= _nextFireTime && enemyDetector)
        {
            enemyDetector.CleanUpEnemies();

            if (enemyDetector.EnemiesInRange.Count > 0)
            {
                _target = GetClosestEnemy(enemyDetector.EnemiesInRange);
                
                if (_target)
                {
                    _playerMovement.SetLockedTarget(_target.transform);
                    if (Vector3.Angle(_target.transform.forward,transform.forward) <= 180)
                    {
                        FireAt();
                        _nextFireTime = Time.time + (1f / fireRate);
                    
                        
                    }
                    
                    targetIndicator.transform.parent = _target.transform;
                    targetIndicator.transform.localPosition = Vector3.zero + Vector3.up * 0.05f;
                }
            }
        }
        
        targetIndicator.gameObject.SetActive(_target && !_target.IsDead);
        
    }

 
    private Enemy GetClosestEnemy(List<Enemy> enemies)
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 shooterPosition = transform.position;

        foreach (Enemy enemy in enemies)
        {
            if (!enemy) continue; // Skip any null entries

            float distance = Vector3.Distance(shooterPosition, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        
        return closestEnemy;
    }

    private void FireAt()
    {
        
        if (Physics.Raycast(muzzle.position, -muzzle.forward,out RaycastHit hit,Mathf.Infinity,layerMask:data.targetLayer))
        {
            if (hit.transform.TryGetComponent(out Enemy _))
            {
                
                var bullet = _bulletPool.Get();
                if (bullet is null) { return; }
                
                //enemy.TakeDamage(data.damage);
                
                
                bullet.hitInfo = hit;
                bullet.transform.position = muzzle.position;
                bullet.ApplyForce((hit.point - bullet.transform.position).normalized);
                muzzleFlash.Play();

                bullet.Deactivate();


            }
        }
        
        
        

        
        
        
        
    }
    
}
