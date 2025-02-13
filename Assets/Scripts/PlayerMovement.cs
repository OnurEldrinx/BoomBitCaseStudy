using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    
    #region Private Fields
    
    private Animator _animator;
    private Vector2 _movementDirection;
    private Transform _lockedTarget;

    #endregion
    
    #region Serialized Fields
    
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lockingTargetSpeed;
    [SerializeField] private float animationSmoothingTime = 0.1f;
    
    #endregion
    
    #region Animator Hashes
    
    private static readonly int InputX = Animator.StringToHash("InputX");
    private static readonly int InputY = Animator.StringToHash("InputY");

    #endregion
    
    private void OnEnable()
    {
        EnemyDetector.EnemyOutOfRange += enemy =>
        {
            if (_lockedTarget == null)
            {
                return;
            }
            var e = _lockedTarget.GetComponent<Enemy>();
            if (e == enemy)
            {
                _lockedTarget = null;
            }
        };
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (_lockedTarget && _lockedTarget.gameObject.activeInHierarchy)
        {
            LockOnTarget(_lockedTarget);
        }
        else
        {
            Rotate();
        }
        
        ApplyMovement();
        
    }

    private void ApplyMovement()
    {
        var inputX = _movementDirection.x;
        var inputY = _movementDirection.y;
        
        var worldDir = new Vector3(inputX,0,inputY);
        transform.position += worldDir * Time.deltaTime;
       
        var localDir = transform.InverseTransformDirection(worldDir.normalized);
        
        _animator.SetFloat(InputX, localDir.x * 1f,animationSmoothingTime,Time.deltaTime);
        _animator.SetFloat(InputY, localDir.z * 1f,animationSmoothingTime,Time.deltaTime);
    }
    
    private void Rotate()
    {
        if (_movementDirection.magnitude < 0.1f)
        {
            return;
        }
            
        var smooth = rotationSpeed * Time.smoothDeltaTime;
        var targetRotation = Quaternion.LookRotation(new Vector3(_movementDirection.x,0,_movementDirection.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smooth);
    }

    private void LockOnTarget(Transform target)
    {
        //transform.LookAt(new Vector3(target.position.x,transform.position.y,target.position.z));
        
        Vector3 targetDirection = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
    
        if (targetDirection.sqrMagnitude < 0.1f) return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockingTargetSpeed * Time.deltaTime);
        
    }

    public void SetLockedTarget(Transform target)
    {
        _lockedTarget = target;
    }
    
    
    #region INPUT

    public void OnMove(InputValue value)
    {
        _movementDirection = value.Get<Vector2>().normalized;
    }

    #endregion
}
