using System;
using UnityEngine;
using UnityEngine.Pool;

public class VFXManager : Singleton<VFXManager>
{
    [SerializeField] private ParticleEffectCallback bloodSplash;
    [SerializeField] private ParticleEffectCallback bloodExplosion;
    
    public IObjectPool<ParticleEffectCallback> BloodSplashPool { get; set; }
    public IObjectPool<ParticleEffectCallback> BloodExplosionPool { get; set; }

    [SerializeField] private int capacity = 50;
    [SerializeField] private int maxCapacity = 100;
    private readonly bool _poolCheckA = true;
    private readonly bool _poolCheckB = true;
    
    private void Awake()
    {
        BloodSplashPool = new ObjectPool<ParticleEffectCallback>(CreateBloodSplash,OnGetBloodSplash,OnReturnBloodSplash,OnDestroyBloodSplash,_poolCheckA,capacity,maxCapacity);
        BloodExplosionPool = new ObjectPool<ParticleEffectCallback>(CreateBloodExplosion,OnGetBloodExplosion,OnReturnBloodExplosion,OnDestroyBloodExplosion,_poolCheckB,capacity,maxCapacity);
        
    }

    private void OnDestroyBloodExplosion(ParticleEffectCallback obj)
    {
        Destroy(obj.gameObject);
    }

    private void OnReturnBloodExplosion(ParticleEffectCallback obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGetBloodExplosion(ParticleEffectCallback obj)
    {
        obj.gameObject.SetActive(true);
    }

    private ParticleEffectCallback CreateBloodExplosion()
    {
        var p = Instantiate(bloodExplosion);
        p.Pool = BloodExplosionPool;
        return p;
    }

    private void OnDestroyBloodSplash(ParticleEffectCallback obj)
    {
        Destroy(obj.gameObject);
    }

    private void OnReturnBloodSplash(ParticleEffectCallback obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnGetBloodSplash(ParticleEffectCallback obj)
    {
        obj.gameObject.SetActive(true);
    }

    private ParticleEffectCallback CreateBloodSplash()
    {
        var p = Instantiate(bloodSplash);
        p.Pool = BloodSplashPool;
        return p;
    }
}

public enum VFXType
{
    BloodSplash,
    BloodExplosion
}
