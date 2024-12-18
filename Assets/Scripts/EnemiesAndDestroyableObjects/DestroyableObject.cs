using System;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int hitPoints = 3; 
    [SerializeField] private ParticleSystem explosionEffect; 

    private int currentHits = 0;

    public static event Action<GameObject> OnObjectDestroyed;

    public void TakeDamage()
    {
        currentHits++;
        Debug.Log($"{gameObject.name} hit {currentHits}/{hitPoints} times.");

        if (currentHits >= hitPoints)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        if (explosionEffect != null)
        {
            ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration);
        }

        OnObjectDestroyed?.Invoke(gameObject); 

        Destroy(gameObject);
    }
}
