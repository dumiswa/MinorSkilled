using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject _armsRigPrefab;
    [SerializeField] private GameObject _gunPrefabVariant;
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
    }

    private void AttachArmsAndGun()
    {
        if (_armsRigPrefab != null)
        {
            GameObject armsRig = Instantiate(_armsRigPrefab, _player.transform.Find("Main Camera"));
            Transform weaponMountPoint = armsRig.transform.Find("WeaponMountPoint");

            PlayerController playerController = _player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetArmsRig(armsRig.transform);
            }

            if (weaponMountPoint != null && _gunPrefabVariant != null)
            {
                GameObject gun = Instantiate(_gunPrefabVariant, weaponMountPoint);
            }
            else Debug.LogError("WeaponMountPoint or selected gun is missing" + weaponMountPoint + _gunPrefabVariant);
        }
    }
}
