using System.Collections;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private Transform gunTip; 
    [SerializeField] private ParticleSystem muzzleFlash; 
    [SerializeField] private AudioSource shootSound; 
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float maxRaycastDistance = 100f; 

    private float nextTimeToFire = 0f;
    private Camera playerCamera;

    [Header("Recoil Settings")]
    [SerializeField] private WeaponRecoil recoilSystem; 

    [Header("Input Settings")]
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

    private void Start()
        => AssignCamera();

    private void AssignCamera()
    {    
        playerCamera = Camera.main;
    }


    private void Update()
    {
        if (SceneLoader.IsSceneLoaded)       
            HandleShooting();      
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
        if (playerCamera == null)
        {
            AssignCamera(); // Ensure the camera is reassigned dynamically if it is null
            if (playerCamera == null)
            {
                Debug.LogError("Main Camera not found for shooting system.");
                return;
            }
        }

        PlayMuzzleFlash();
        ApplyRecoil();
        PlayShootSound();

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);

            DestroyableObject destroyable = hit.collider.GetComponent<DestroyableObject>();
            if (destroyable != null)      
                destroyable.TakeDamage();          

            KillableDummy dummy = hit.collider.GetComponent<KillableDummy>();
            if (dummy != null)
                dummy.TakeDamage();    
        }
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Clear();
            muzzleFlash.Play();
            StartCoroutine(StopMuzzleFlashAfterDuration(0.1f));
        }
    }

    private void ApplyRecoil()
    {
        if (recoilSystem != null)     
            recoilSystem.ApplyRecoil();
        
    }

    private void PlayShootSound()
    {
        if (shootSound != null)
            shootSound.Play();
        
    }

    private IEnumerator StopMuzzleFlashAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        muzzleFlash.Stop();
    }
}
