using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public static event Action<CollectableItem> OnItemCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
