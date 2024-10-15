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

        /*if (PlayerData.selectedGunPrefab == null)
        {
            GameObject gunToSpawnWith = Resources.Load<GameObject>("G");
            PlayerData.selectedGunPrefab = gunToSpawnWith;
            Debug.Log("Loaded gun from Resources.");
        }*/

        AttachArmsAndGun();
    }


    private void AttachArmsAndGun()
    {
        if (_armsRigPrefab != null)
        {
            GameObject armsRig = Instantiate(_armsRigPrefab, _player.transform.Find("Main Camera"));
            Transform weaponMountPoint = armsRig.transform.Find("WeaponMountPoint");
            Debug.Log("Found weapon mount point: " + weaponMountPoint);

            PlayerController playerController = _player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetArmsRig(armsRig.transform);  // Set the arms rig on the player
            }

            if (weaponMountPoint != null && PlayerData.SelectedGunPrefab != null)
            {
                GameObject gun = PlayerData.SelectedGunPrefab;

                // Attach the weapon to the WeaponMountPoint
                gun.transform.SetParent(weaponMountPoint);  // Attach gun to mount point
                gun.transform.localPosition = Vector3.zero;  // Reset position
                gun.transform.localRotation = Quaternion.identity;  // Reset rotation

                Debug.Log("Instantiated selected gun: " + gun.name);
            }
            else
            {
                Debug.LogError(PlayerData.SelectedGunPrefab);
            }
        }
    }
}
