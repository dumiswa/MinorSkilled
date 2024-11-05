using System;
using UnityEngine;


[CreateAssetMenu()]
public class WeaponBodySO : ScriptableObject
{
    public enum Body
    {
        Colt,
        Ak,
        Pistol,
    }

    public Body body;
    public Transform prefab;
    public Transform prefabUI;
    public WeaponPartListSO weaponPartListSO;
}
