using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public Enemy enemyPrefab;

    [SerializeField] private float startDelay;
    [SerializeField] private float waveInterval;

    private int _currentWave;
    private int _waveCount;

    private LevelData _currentLevelData;
    
    private NavMeshTriangulation _triangulation;

    private IObjectPool<Enemy> _enemyPool;
    private readonly bool _poolCheck = true;
    [SerializeField] private int capacity = 50;
    [SerializeField] private int maxCapacity = 100;
    
    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy,OnGetEnemy,OnReturnEnemy,OnDestroyEnemy,_poolCheck,capacity,maxCapacity);

    }

    private void OnDestroyEnemy(Enemy obj)
    {
        Destroy(obj);
    }

    private void OnReturnEnemy(Enemy obj)
    {
        obj.Agent.enabled = false;
        obj.gameObject.SetActive(false);
    }

    private void OnGetEnemy(Enemy obj)
    {
        obj.gameObject.SetActive(true);
        obj.Reset();
    }

    private Enemy CreateEnemy()
    {
        var e = Instantiate(enemyPrefab);
        e.Pool = _enemyPool;
        return e;    
    }


    private void OnEnable()
    {
        GameManager.Instance.OnWin += () =>
        {
            _currentWave = 0;
            _currentLevelData = null;
        };
        GameManager.Instance.OnNextLevel += () =>
        {
            _currentWave = 0;
            _currentLevelData = null;
        };
    }
    

    private void Start()
    {
        _triangulation = NavMesh.CalculateTriangulation();
    }

    public void StartSpawning(LevelData levelData,float difficultyMultiplier)
    {
        _currentLevelData = levelData;
        waveInterval = levelData.timeBetweenWaves;
        _waveCount = levelData.waveCountTable.Count;
        
        levelData.enemyType.damage *= (int)Mathf.Pow(difficultyMultiplier,2);
        levelData.enemyType.health *= (int)Mathf.Pow(difficultyMultiplier,2);
        //levelData.enemyType.pathUpdateInterval /= Mathf.Pow(difficultyMultiplier,2);
        
        StartCoroutine(SpawnWaves());
    }
    
    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startDelay);

        while (_currentWave < _waveCount)
        {
            for (int i = 0; i < _currentLevelData.waveCountTable[_currentWave]; i++)
            {
                SpawnEnemy(_currentLevelData.enemyType);
                yield return new WaitForSeconds(0.1f);
            }
            _currentWave++;
            yield return new WaitForSeconds(waveInterval);
        }

    }

    private void SpawnEnemy(EnemyData data)
    {
        DataManager.Instance.TotalEnemyCount++;
        Enemy enemy = Instantiate(enemyPrefab);
        //Enemy enemy = _enemyPool.Get();
        enemy.BindData(data);
        
        int vertexIndex = Random.Range(0, _triangulation.vertices.Length);

        if (NavMesh.SamplePosition(_triangulation.vertices[vertexIndex], out var hit, 2f, -1))
        {
            enemy.Agent.Warp(hit.position);
            enemy.Agent.enabled = true;
            enemy.StartChasing();
        }
        
    }
}
