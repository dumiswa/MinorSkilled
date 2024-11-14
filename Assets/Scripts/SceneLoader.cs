using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
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

        WeaponAttachmentSystem.Instance?.DisableAttachmentMenuUI();
    }


    private void AttachArmsAndGun()
    {
        if (_armsRigPrefab != null)
        {
            // Instantiate arms rig
            GameObject armsRig = Instantiate(_armsRigPrefab, _player.transform.Find("Main Camera"));
            Transform weaponMountPoint = armsRig.transform.Find("WeaponMountPoint");

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
                        // Reparent the child to the WeaponMountPoint
                        weaponChild.SetParent(weaponMountPoint, false);

                        // Reset position and rotation
                        weaponChild.localPosition = Vector3.zero;
                        weaponChild.localRotation = Quaternion.Euler(0, 180, 0);

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
                                    Debug.LogError("IKHandler not found on arms rig!");
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
                }           
            }         
        }
    }
}
