using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum PartType
{
    Scope,
    Muzzle,
    BackGrip,
    FrontGrip,
    Handguard,
    Magazine,
    Stock,
    Trigger,
}
public class WeaponBuilder : MonoBehaviour
{
    public BaseWeapon BaseWeapon;
    public WeaponPart SelectedBackGrip;
    public WeaponPart SelectedFrontGrip;
    public WeaponPart SelectedHandguard;
    public WeaponPart SelectedMagazine;
    public WeaponPart SelectedMuzzle;
    public WeaponPart SelectedScope;
    public WeaponPart SelectedStock;
    public WeaponPart SelectedTrigger;

    [SerializeField] private Material _weaponMaterial;
    [SerializeField] private GameObject _finalWeaponModel;

    private float _finalDamage;
    private float _finalFireRate;
    private float _finalRecoil;
    private float _finalWeight;
    private float _finalReloadTime;

    public WeaponPart[] availableScopes;
    public WeaponPart[] availableMuzzles;
    public WeaponPart[] availableBackGrips;
    public WeaponPart[] availableFrontGrips;
    public WeaponPart[] availableMagazines;
    public WeaponPart[] availableStocks;
    public WeaponPart[] availableTriggers;
    public WeaponPart[] availableHandguards;

    private GameObject currentBackGrip;
    private GameObject currentFrontGrip;
    private GameObject currentHandguard;
    private GameObject currentMagazine;
    private GameObject currentMuzzle;
    private GameObject currentScope;
    private GameObject currentStock;
    private GameObject currentTrigger;

    [SerializeField] private Transform _backGripParent;
    [SerializeField] private Transform _frontGripParent;
    [SerializeField] private Transform _handguardParent;
    [SerializeField] private Transform _magazineParent;
    [SerializeField] private Transform _muzzleParent;
    [SerializeField] private Transform _scopeParent;
    [SerializeField] private Transform _stockParent;
    [SerializeField] private Transform _triggerParent;

    public WeaponPart[] GetAvailablePartsByType(PartType partType)
    {
        Debug.Log($"Fetching available parts for: {partType}");

        switch (partType)
        {
            case PartType.Scope:             
                return availableScopes;
            case PartType.Muzzle:
                return availableMuzzles;
            case PartType.BackGrip:
                return availableBackGrips;
            case PartType.FrontGrip:
                return availableFrontGrips;
            case PartType.Magazine:
                return availableMagazines;
            case PartType.Stock:
                return availableStocks;
            case PartType.Handguard:
                return availableHandguards;
            case PartType.Trigger:
                return availableTriggers;
        }

        return null;
    }

    public void ReplacePart(GameObject partModel, ref GameObject currentPart, string partTag)
    {
        if (_finalWeaponModel == null)
        {
            Debug.LogError("No final weapon model assigned!");
            return;
        }

        // Enable the selected part
        partModel.SetActive(true);
        Debug.Log($"Enabled part: {partModel.name}");

        // Find all parts with the same tag
        GameObject[] partsWithSameTag = GameObject.FindGameObjectsWithTag(partTag);

        // Disable all parts with the same tag, except the partModel we want to enable
        foreach (GameObject part in partsWithSameTag)
        {
            if (part != partModel) // Exclude the part we're enabling
            {
                part.SetActive(false);
                Debug.Log($"Disabled part: {part.name}");
            }
        }

        // Set the current part reference to the selected part
        currentPart = partModel;
    }

}

