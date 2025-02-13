using System;
using DG.Tweening;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public Animator animator; // Player's Animator component
    public Transform leftHandTarget; // The transform where the left hand should attach (on the weapon)
    public Transform rightHandTarget;
    public float ikWeight = 1f; // Strength of IK effect
    public bool enableIK = true; // Toggle IK dynamically
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator != null && enableIK && leftHandTarget != null)
        {
            // Set IK weight for the left hand
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);

            // Assign the target position & rotation
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            
            // Set IK weight for the left hand
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);

            // Assign the target position & rotation
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            
        }
        else
        {
            // Reset IK when disabled
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }

    public void SetAimIK(float weight)
    { 
        DOTween.To(()=> ikWeight, x=> ikWeight = x, weight, 0.75f);
    }
}