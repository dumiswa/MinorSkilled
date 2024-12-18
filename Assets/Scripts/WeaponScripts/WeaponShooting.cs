using System.Collections;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private Transform gunTip; 
    [SerializeField] private ParticleSystem muzzleFlash; 
    [SerializeField] private AudioSource shootSound; 
    [SerializeField] private float fireRate = 0.2f; 
    private float nextTimeToFire = 0f;

    [Header("Recoil Settings")]
    [SerializeField] private WeaponRecoil recoilSystem; 

    [Header("Input Settings")]
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0; 

    private void Update()
    {
        if (SceneLoader.IsSceneLoaded) 
        {
            HandleShooting();
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKey(shootKey) && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + fireRate; 
        }
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Clear();
            muzzleFlash.transform.localRotation = Quaternion.Euler(
                Random.Range(0f, 360f), 
                90,
                0
            );
            muzzleFlash.Play();
            StartCoroutine(StopMuzzleFlashAfterDuration(0.1f));
        }    
        if (shootSound != null)       
            shootSound.Play();      
        if (recoilSystem != null)
            recoilSystem.ApplyRecoil();
    }
    private IEnumerator StopMuzzleFlashAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        muzzleFlash.Stop();
    }
}
