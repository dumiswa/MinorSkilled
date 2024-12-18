using System;
using UnityEngine;

public class KillableDummy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int hitPoints = 3;

    private int currentHits = 0;
    private Rigidbody[] ragdollBodies;
    private Animator animator;

    public static event Action<GameObject> OnEnemyKilled;

    private void Start()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>(); 

        SetRagdollState(false);
    }

    public void TakeDamage()
    {
        currentHits++;

        if (currentHits >= hitPoints)
        {
            EnableRagdoll();
            OnEnemyKilled?.Invoke(gameObject); 
        }
    }

    private void EnableRagdoll()
    { 
        if (animator != null)      
            animator.enabled = false;
        
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
        }

        SetRagdollState(true);
    }

    private void SetRagdollState(bool state)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
        }
    }
}
