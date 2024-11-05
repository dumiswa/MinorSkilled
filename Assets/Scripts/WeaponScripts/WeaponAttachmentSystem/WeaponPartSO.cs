using System;
using UnityEngine;

public class WeaponPartSO : ScriptableObject
{
    public enum PartType
    {
        HandGuard,
        Muzzle,
        FrontGrip,
        Stock,
        BackGrip,
        Magazine,
        Scope,   
    }

    public PartType partType;
    public Transform prefab;
}