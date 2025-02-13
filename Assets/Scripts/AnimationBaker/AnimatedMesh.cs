using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AnimatedMesh : MonoBehaviour
{
    [SerializeField]
    private AnimatedMeshScriptableObject AnimationSO;
    private MeshFilter Filter;

    [Header("Debug")]
    [SerializeField]
    private int Tick = 1;
    [SerializeField]
    private int AnimationIndex;
    [SerializeField]
    private string AnimationName;
    private List<Mesh> AnimationMeshes;

    public delegate void AnimationEndEvent(string Name);
    public event AnimationEndEvent OnAnimationEnd;

    private float LastTickTime;


    private void Awake()
    {
        Filter = GetComponent<MeshFilter>();
    }

    public void Play(string stateName)
    {
        if (stateName != AnimationName)
        {
            AnimationName = stateName;
            Tick = 1;
            AnimationIndex = 0;
            AnimatedMeshScriptableObject.Animation a = AnimationSO.Animations.Find((item) => item.Name.Equals(AnimationName));
            AnimationMeshes = a.Meshes;
            if (string.IsNullOrEmpty(a.Name))
            {
                print($"Animated model {name} does not have an animation baked for {AnimationName}!");
            }
        }
    }

    private void Update()
    {
        if (AnimationMeshes != null)
        {
            if (Time.time >= LastTickTime + (1f / AnimationSO.AnimationFPS))
            {
                Filter.mesh = AnimationMeshes[AnimationIndex];

                AnimationIndex++;
                if (AnimationIndex >= AnimationMeshes.Count)
                {
                    OnAnimationEnd?.Invoke(AnimationName);
                    AnimationIndex = 0;
                }
                LastTickTime = Time.time;
            }
            Tick++;
        }
    }
}
