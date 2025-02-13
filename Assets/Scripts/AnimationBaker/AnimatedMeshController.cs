using UnityEngine;

public class AnimatedMeshController : MonoBehaviour
{
    private AnimatedMesh[] Animators;
    private void Awake()
    {
        Animators = FindObjectsOfType<AnimatedMesh>();
    }

    public void DeactivateAll()
    {
        foreach (AnimatedMesh animator in Animators)
        {
            animator.enabled = false;
            animator.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    public void ActivateAll()
    {
        foreach (AnimatedMesh animator in Animators)
        {
            animator.enabled = true;
            animator.GetComponentInChildren<MeshRenderer>().enabled = true;
            //animator.Play("Run_S");
        }
    }
    
}
