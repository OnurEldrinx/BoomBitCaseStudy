using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private static readonly int Move = Animator.StringToHash("Move");

    //private Animator _animator;
    [SerializeField] private AnimatedMesh _animatedMesh;
    private CapsuleCollider _collider;
    public NavMeshAgent Agent { get; private set; }
    private Transform _playerTransform;
    [SerializeField]private float _health;
    private float _pathUpdateInterval;
    private float _attackDamage;
    private Vector2 _rootMotionVelocity;
    private Vector2 _rootMotionSmoothDeltaPosition;
    private Coroutine _chaseCoroutine;
    
    public bool IsDead { get; private set; }
    [field: SerializeField] public EnemyData Data { get; private set; }

    public IObjectPool<Enemy> Pool;
    
    private void Awake()
    {
        //_animator = GetComponent<Animator>();
        _animatedMesh = GetComponent<AnimatedMesh>();
        _collider = GetComponent<CapsuleCollider>();
        Agent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        Agent.updatePosition = false;
        Agent.updateRotation = true;
        
    }

    private void OnEnable()
    {
        GameManager.Instance.OnWin += () =>
        {
            gameObject.SetActive(false);
        };
        
        GameManager.Instance.OnNextLevel += () =>
        {
            gameObject.SetActive(false);
        };
    }


    public void BindData(EnemyData d)
    {
        Data = d;
        _pathUpdateInterval = Data.pathUpdateInterval;
        _health = Data.health;
        _attackDamage  = Data.damage;
        //transform.localScale *= data.bodyScaleFactor;
    }

    public void Reset()
    {
        if (Data != null)
        {
            _health = Data.health;
            _attackDamage  = Data.damage;
        }
        
        _collider.enabled = true;
        Agent.enabled = true;
        Agent.SetDestination(_playerTransform.position);
    }

    private void Update()
    {
        SyncAnimatorWithMovement();
    }
    
    /*private void OnAnimatorMove()
    {
        var rootPosition = _animator.rootPosition;
        rootPosition.y = _agent.nextPosition.y;
        transform.position = rootPosition;
        _agent.nextPosition = rootPosition;
    }*/
    
    private void SyncAnimatorWithMovement()
    {
        var t = transform;
        var currentDeltaPosition = Agent.nextPosition - t.position;
        currentDeltaPosition.y = 0;

        var deltaX = Vector3.Dot(t.right, currentDeltaPosition);
        var deltaY = Vector3.Dot(t.forward, currentDeltaPosition);

        var newDeltaPosition = new Vector2(deltaX,deltaY);
        var s = Mathf.Min(1,Time.deltaTime/10f);

        _rootMotionSmoothDeltaPosition = Vector2.Lerp(_rootMotionSmoothDeltaPosition, newDeltaPosition, s);
        _rootMotionVelocity = _rootMotionSmoothDeltaPosition / Time.deltaTime;

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            _rootMotionVelocity = Vector2.Lerp(Vector2.zero, _rootMotionVelocity, Agent.remainingDistance / Agent.stoppingDistance);
        }

        //bool move = _rootMotionVelocity.magnitude > 0.05f && _agent.remainingDistance > _agent.stoppingDistance;
        bool move = Agent.remainingDistance > Agent.stoppingDistance;
        
        //_animator.SetBool(Move,move);

        _animatedMesh.Play(move ? "Run" : "Attack");
        transform.position = Agent.nextPosition;
        //_animator.SetFloat(MovementBlend,_rootMotionVelocity.magnitude);

        //var deltaPositionMagnitude = currentDeltaPosition.magnitude;

        /*if (deltaPositionMagnitude > _agent.radius/2f)
        {
            transform.position = Vector3.Lerp(_animator.rootPosition, _agent.nextPosition,s);
        }*/


    }

    private void DisableDetection()
    {
        _collider.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
           OnDeath();
        }
    }

    private void OnDeath()
    {
        IsDead = true; 
        DisableDetection();
        gameObject.SetActive(false);
        DataManager.Instance.UpdateDefeatedEnemiesTable(1);
        
        //var deathEffect = Instantiate(Data.deathEffect, transform.position + Vector3.up, Quaternion.identity);
        var deathEffect = VFXManager.Instance.BloodExplosionPool.Get();
        deathEffect.transform.position = transform.position + Vector3.up;
        deathEffect.PlayEffect();
    }
    
    public void StartChasing()
    {
        if (_chaseCoroutine == null)
        {
            _chaseCoroutine = StartCoroutine(ChasePlayer());
        }
    }
    
    private IEnumerator ChasePlayer()
    {
        var wait = new WaitForSeconds(_pathUpdateInterval);

        while (gameObject.activeSelf)
        {
            Agent.SetDestination(_playerTransform.position);
            yield return wait;
        }
    }

    private void OnDisable()
    {
        Agent.enabled = false;
        //Pool.Release(this);
    }
    
    
}
