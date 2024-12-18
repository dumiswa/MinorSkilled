using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
    public static bool IsSceneLoaded { get; private set; } = false;

    [SerializeField] private GameObject _armsRigPrefab;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");

        if (_player == null)
        {
            Debug.LogError("Player not found in the scene!");
            return;
        }

        AttachArmsAndGun();

        IsSceneLoaded = true;
        WeaponAttachmentSystem.Instance?.DisableAttachmentMenuUI();
    }


    private void AttachArmsAndGun()
    {
        if (_armsRigPrefab != null)
        {
            // Instantiate arms rig
            GameObject armsRig = Instantiate(_armsRigPrefab, _player.transform.Find("Main Camera"));
            Transform weaponMountPoint = armsRig.transform.Find("WeaponMountPoint");
            weaponMountPoint.localRotation = Quaternion.Euler(0f, -180f, 0f);


            Debug.Log("Found weapon mount point: " + weaponMountPoint);

            // Attach arms rig to the player
            PlayerController playerController = _player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetArmsRig(armsRig.transform);
            }

            // Attach only the child with the tag "Weapon"
            if (weaponMountPoint != null && WeaponAttachmentSystem.Instance != null)
            {
                WeaponComplete weapon = WeaponAttachmentSystem.Instance.GetWeaponComplete();
                if (weapon != null)
                {
                    // Find the child with the "Weapon" tag
                    Transform weaponChild = null;
                    foreach (Transform child in weapon.transform)
                    {
                        if (child.CompareTag("Weapon"))
                        {
                            weaponChild = child;
                            break;
                        }
                    }

                    if (weaponChild != null)
                    {
                        //Quaternion originalLocalRotation = weaponChild.localRotation;
                        // Reparent the child to the WeaponMountPoint
                        weaponChild.SetParent(weaponMountPoint, false);

                        // Reset position and rotation
                        weaponChild.localPosition = Vector3.zero;
                        weaponChild.localRotation = Quaternion.Euler(0, 0, 0);
                        //weaponChild.localRotation = originalLocalRotation;

                        Debug.Log("Weapon successfully attached to mount point: " + weaponChild.name);
                     
                        Transform attachPoints = weaponChild.Find("AttachPoints");
                        if (attachPoints != null)
                        {
                            Transform frontGrip = attachPoints.Find("FrontGrip");
                            Transform backGrip = attachPoints.Find("BackGrip");                            

                            if (frontGrip != null && backGrip != null)
                            {
                                // Assign the grips to the IKHandler
                                IKHandler ikHandler = armsRig.GetComponent<IKHandler>();
                                if (ikHandler != null)
                                {
                                    Debug.Log("Initializing IKHandler...");
                                    //ikHandler.Initialize(); // Explicitly initialize before setting targets
                                    ikHandler.SetHandTargets(frontGrip, backGrip);
                                    Debug.Log("Hand targets set for IK.");
                                }
                                else
                                {
                                    //Debug.LogError("IKHandler not found on arms rig!");
                                }
                            }
                            else
                            {
                                Debug.LogError("FrontGrip or BackGrip not found under Attachpoints!");
                            }
                        }
                        else
                        {
                            Debug.LogError("Attachpoints not found on the weapon!");
                        }
                    }
                    AddWeaponADSScript(weaponChild.gameObject, _player.GetComponentInChildren<Camera>());
                }           
            }
            
        }
    }

    private void AddWeaponADSScript(GameObject weapon, Camera playerCamera)
    {
        Debug.Log("hello");
        // Check if the WeaponADS script is already attached
        if (weapon.GetComponent<WeaponADS>() == null)
        {
            // Add the WeaponADS script
            WeaponADS weaponADS = weapon.AddComponent<WeaponADS>();
  
            // Configure the WeaponADS script's variables
            weaponADS._weaponADSLayer = weapon.transform; // Assuming the scope moves for ADS
            weaponADS.smoothTime = 20f; // Example smooth value
            weaponADS.offsetX = 0f;    // Adjust for proper ADS alignment
            weaponADS.offsetY = -0.05f;
            weaponADS.offsetZ = 0.2f;  // Pull the weapon closer to the camera
            weaponADS.ADSKey = KeyCode.Mouse1;

            // Assign the player's camera to the script
            weaponADS.GetType().GetField("_camera", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(weaponADS, playerCamera);      

            Debug.Log("WeaponADS script added and configured on weapon: " + weapon.name);
        }
        else
        {
            Debug.LogWarning("WeaponADS script is already attached to the weapon!");
        }
    }

    private void AddWeaponSway(GameObject weapon)
    {
        if (weapon.GetComponent<WeaponSway>() == null)
        {
            WeaponSway weaponSway = weapon.AddComponent<WeaponSway>();
        }
    }
}
