using UnityEngine;

[CreateAssetMenu(fileName = "Muzzle", menuName = "Weapon/Part/Muzzle")]
public class Muzzle : WeaponPart
{
    public float SoundModifier;
    public float RangeModifier;
    public float WeightModifier;
    public float RecoilModifier;
    public float DamageModifier;
}
