using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public GameObject selectedGunPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}