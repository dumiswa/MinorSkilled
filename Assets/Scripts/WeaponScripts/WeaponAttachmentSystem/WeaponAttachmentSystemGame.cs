using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponAttachmentSystemGame : MonoBehaviour
{
    public static string WeaponCompleteJason;

    [SerializeField] private Button _backButton;
    [SerializeField] private Transform _weaponContainerTransform;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private AnimatorController _pistonAnimatorController;
    [SerializeField] private AnimatorController _rifleAnimatorController;
    [SerializeField] private WeaponBodyListSO _weaponBodyListSO;

    private void Awake()
    {
        Destroy(_weaponContainerTransform.GetChild(0).gameObject);

        _backButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });

        if (WeaponCompleteJason == null) return;

        WeaponComplete weaponComplete = WeaponComplete.LoadSpawnWeaponComplete(WeaponCompleteJason, false);
        weaponComplete.transform.parent = _weaponContainerTransform;
        weaponComplete.transform.localPosition = Vector3.zero;
        weaponComplete.transform.localEulerAngles = Vector3.zero;

        if (weaponComplete.GetWeaponBodySO() == _weaponBodyListSO.pistolWeaponBodySO)     
            _playerAnimator.runtimeAnimatorController = _pistonAnimatorController;
        
        if (weaponComplete.GetWeaponBodySO() == _weaponBodyListSO.akWeaponBodySO ||
            weaponComplete.GetWeaponBodySO() == _weaponBodyListSO.coltWeaponBodySO)   
            _playerAnimator.runtimeAnimatorController = _rifleAnimatorController;
        

    }
}
