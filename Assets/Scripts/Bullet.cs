using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public IObjectPool<Bullet> Pool { get; set; }
    private Rigidbody _rb;
    private TrailRenderer _trail;
    
    [SerializeField] private float velocity;
    [SerializeField] private float deactivateDelay;

    public RaycastHit hitInfo;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();
        _trail.emitting = false;
    }

    public async void ApplyForce(Vector3 direction)
    {
        try
        {
            _rb.AddForce(direction * velocity, ForceMode.Acceleration);
            await Task.Delay(20);
            _trail.emitting = true;
        }
        catch
        {
            // ignored
        }
    }
    
    public void Deactivate()
    {
        StartCoroutine(DeactivateCoroutine(deactivateDelay));
    }

    private IEnumerator DeactivateCoroutine(float d)
    {
        yield return new WaitForSeconds(d);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        
        Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(1);

            var bloodSplash = VFXManager.Instance.BloodSplashPool.Get();
            if (!bloodSplash) { return; }
            bloodSplash.transform.position = transform.position;
            bloodSplash.transform.forward = hitInfo.normal;
            bloodSplash.PlayEffect();
            
            Disable();
        }
    }
    
    private void Disable(){
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        Pool.Release(this);
        _trail.emitting = false;
    }
}
