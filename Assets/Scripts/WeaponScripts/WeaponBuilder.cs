using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _gunHolder;  

    public void ChangeWeaponPart(GameObject selectedPart)
    {
        Transform parentSocket = selectedPart.transform.parent;

        foreach (Transform child in parentSocket)
        {
            child.gameObject.SetActive(false);  
        }

        selectedPart.SetActive(true);  
    }

    public void SaveModifiedGun()
    {
        if (_gunHolder != null && _gunHolder.transform.childCount > 0)
        {
            GameObject weapon = _gunHolder.transform.GetChild(0).gameObject;
            weapon.transform.SetParent(null);
            PlayerData.SelectedGunPrefab = weapon; 
            DontDestroyOnLoad(weapon);
            Debug.Log("Weapon saved and will persist between scenes: " + weapon.name);
        }
        else
        {
            Debug.LogError("Weapon not found in GunHolder!");
        }
    }
}
