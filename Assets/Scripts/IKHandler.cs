using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKHandler : MonoBehaviour
{
    [Header("IK Settings")]
    // Left Hand Settings
    public Transform palmLeft; // Assign the "palm left" from the rig hierarchy
    public Transform leftElbowHint; // Optional elbow hint for better bending

    // Right Hand Settings
    public Transform palmRight; // Assign the "palm right" from the rig hierarchy
    public Transform rightElbowHint; // Optional elbow hint for better bending

    private TwoBoneIKConstraint leftArmIK; // Left arm IK constraint
    private TwoBoneIKConstraint rightArmIK; // Right arm IK constraint

    private void Awake()
    {
        // Find the Two Bone IK Constraints on the arms
        leftArmIK = transform.Find("root/chest/arm.L").GetComponent<TwoBoneIKConstraint>();
        rightArmIK = transform.Find("root/chest/arm.R").GetComponent<TwoBoneIKConstraint>();

        if (leftArmIK == null)
            Debug.LogError("Two Bone IK Constraint not found on arm left!");

        if (rightArmIK == null)
            Debug.LogError("Two Bone IK Constraint not found on arm right!");

        if (palmLeft == null)
            Debug.LogError("palmLeft is not assigned in the Inspector!");

        if (palmRight == null)
            Debug.LogError("palmRight is not assigned in the Inspector!");
    }

    public void SetHandTargets(Transform frontGrip, Transform backGrip)
    {
        // Ensure all references are valid
        if (leftArmIK == null || rightArmIK == null)
        {
            Debug.LogError("Two Bone IK Constraints are not assigned!");
            return;
        }

        if (frontGrip == null || backGrip == null)
        {
            Debug.LogError("FrontGrip or BackGrip is null!");
            return;
        }

        // Set the left hand target (Front Grip)
        leftArmIK.data.target.position = frontGrip.position;
        leftArmIK.data.target.rotation = frontGrip.rotation;

        // Set the right hand target (Back Grip)
        rightArmIK.data.target.position = backGrip.position;
        rightArmIK.data.target.rotation = backGrip.rotation;

        Debug.Log($"Set Left Hand Target to {frontGrip.name} and Right Hand Target to {backGrip.name}");
        UpdateElbowHints();
    }
    private void UpdateElbowHints()
    {
        if (leftElbowHint != null && leftArmIK != null)
        {
            // Position the left elbow hint slightly behind and to the side of the arm
            Vector3 directionToTarget = (leftArmIK.data.target.position - leftArmIK.transform.position).normalized;
            leftElbowHint.position = leftArmIK.transform.position + directionToTarget * -0.5f + Vector3.up * 0.2f;
            leftArmIK.data.hint = leftElbowHint; // Apply the hint
        }

        if (rightElbowHint != null && rightArmIK != null)
        {
            // Position the right elbow hint slightly behind and to the side of the arm
            Vector3 directionToTarget = (rightArmIK.data.target.position - rightArmIK.transform.position).normalized;
            rightElbowHint.position = rightArmIK.transform.position + directionToTarget * -0.5f + Vector3.up * 0.2f;
            rightArmIK.data.hint = rightElbowHint; // Apply the hint
        }
    }

}

