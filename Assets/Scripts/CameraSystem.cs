using System;
using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void Update()
    {
        transform.position = target.position + offset;
    }
    
    


}
