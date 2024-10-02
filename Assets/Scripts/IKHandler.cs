using UnityEngine;

public class IKHandler : MonoBehaviour
{
    private Animator animator;

    [Header("IK Settings")]
    public Transform leftHand; // For the front grip
    public Transform rightHand; // For the back grip

    [SerializeField] private float _rightHandWeight;
    [SerializeField] private float _leftHandWeight;

    private void Start() => animator = GetComponent<Animator>();

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (rightHand != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightHandWeight); 
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightHandWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
            }

            if (leftHand != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftHandWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
            }
        }
    }
}

