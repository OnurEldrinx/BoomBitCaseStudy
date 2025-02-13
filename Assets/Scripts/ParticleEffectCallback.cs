using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleEffectCallback : MonoBehaviour
{
    
    public IObjectPool<ParticleEffectCallback> Pool;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        Pool.Release(this);
    }
    
    public void PlayEffect(){
        _particleSystem.Play();
    }
}
