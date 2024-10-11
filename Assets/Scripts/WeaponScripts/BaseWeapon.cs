using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="BaseWeapon", menuName = "Base/Weapon")]
public class BaseWeapon : ScriptableObject
{
    public string WeaponName;
    public GameObject WeaponModel;
    public float BaseDamage;
    public float BaseFireRate;
    public float BaseRecoil;
    public float BaseAmmo;
    public float BaseWeight;
    public float BaseReloadTime;
}
