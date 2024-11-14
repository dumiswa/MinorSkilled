using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponAttachmentSystem : MonoBehaviour
{
    public static WeaponAttachmentSystem Instance { get; private set; }

    public event EventHandler OnPartChanged;

    [SerializeField] private WeaponBodyListSO _weaponBodyListSO;
    [SerializeField] private WeaponBodySO _weaponBodySO;
    [SerializeField] private WeaponComplete _weaponComplete;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);  
    }
    private void Start() => SetWeaponBody(_weaponBodySO);
    

    public void SetWeaponBody(WeaponBodySO weaponBodySO)
    {
        Vector3 previousEularAngles = Vector3.zero;

        if(_weaponComplete != null)
        {
            previousEularAngles = _weaponComplete.transform.eulerAngles;
            Destroy(_weaponComplete.gameObject);
        }

        this._weaponBodySO = weaponBodySO;

        // Instantiate and reset weapon body transform
        Transform weaponBodyTransform = Instantiate(_weaponBodySO.prefab, transform);  // Parent to the system for now
        weaponBodyTransform.localPosition = Vector3.zero;  // Reset local position
        weaponBodyTransform.localRotation = Quaternion.identity;  // Reset local rotation
        weaponBodyTransform.eulerAngles = previousEularAngles;  // Apply previous rotation if needed

        // Adjust the weapon model to fit in the UI 
        Transform weaponModel = null;
        foreach (Transform child in weaponBodyTransform.GetComponentInChildren<Transform>(true))
        {
            if (child.CompareTag("Weapon"))
            {
                weaponModel = child;
                weaponModel.localPosition = new Vector3(0, 0.62f, -4.7f);
                weaponModel.localRotation = Quaternion.Euler(0, -90, 0);
                break;          
            }
            else Debug.LogError($"Error in WeaponAttachmentSysatem! No Gameobject with have tag 'Weapon'");
        }

        // Get WeaponComplete component and attach the prefabUI
        _weaponComplete = weaponBodyTransform.GetComponent<WeaponComplete>();
        Transform uiTransform = Instantiate(weaponBodySO.prefabUI, weaponBodyTransform);
        uiTransform.localPosition = Vector3.zero;  // Reset UI position relative to weapon body
        uiTransform.localRotation = Quaternion.identity;  // Reset UI rotation
    }

    public int GetPartIndex(WeaponPartSO.PartType partType)
    {
        WeaponPartSO attachedWeaponPartSO = _weaponComplete.GetWeaponPartSO(partType);
        if (attachedWeaponPartSO == null)      
            return 0;   
        else 
        {
            List<WeaponPartSO> weaponPartSOList = _weaponBodySO.weaponPartListSO.GetWeaponPartSOList(partType); ;
            int partIndex = weaponPartSOList.IndexOf(attachedWeaponPartSO);
            return partIndex;
        }
    }

    public void ChangePart(WeaponPartSO.PartType partType)
    {
        WeaponPartSO attachedWeaponPartSO = _weaponComplete.GetWeaponPartSO(partType);
        if (attachedWeaponPartSO == null)
        {
            _weaponComplete.SetPart(_weaponBodySO.weaponPartListSO.GetWeaponPartSOList(partType)[0]);

            OnPartChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            List<WeaponPartSO> weaponPartSOList = _weaponBodySO.weaponPartListSO.GetWeaponPartSOList(partType);
            int partIndex = weaponPartSOList.IndexOf(attachedWeaponPartSO);
            int nextPartIndex = (partIndex + 1) % weaponPartSOList.Count;
            _weaponComplete.SetPart(weaponPartSOList[nextPartIndex]);

            OnPartChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RandomizeParts()
    {
        foreach (WeaponPartSO.PartType partType in _weaponComplete.GetWeaponPartTypeList())
        {
            int randomAmount = UnityEngine.Random.Range(0, 50);
            for (int i = 0; i < randomAmount; i++)            
                ChangePart(partType);         
        }
    }

    public WeaponBodySO GetWeaponBodySO()
    {
        return _weaponBodySO;
    }

    public void ChangeBody()
    {
        if (_weaponBodySO == _weaponBodyListSO.coltWeaponBodySO)
        {
            SetWeaponBody(_weaponBodyListSO.akWeaponBodySO);
        }
        else
        {
            SetWeaponBody(_weaponBodyListSO.coltWeaponBodySO);
        }
    }

    public WeaponComplete GetWeaponComplete()
    {
        return _weaponComplete;
    }

    public void SetWeaponComplete(WeaponComplete weaponComplete)
    {
        if (this._weaponComplete != null)
        {
            // Clear previous WeaponComplete
            Destroy(this._weaponComplete.gameObject);
        }

        _weaponBodySO = weaponComplete.GetWeaponBodySO();

        this._weaponComplete = weaponComplete;
    }

    public void ResetWeaponRotation()
        => _weaponComplete.transform.eulerAngles = Vector3.zero;

    public void DisableAttachmentMenuUI()
    {
        if (_weaponComplete != null)
        {
            foreach (Transform child in _weaponComplete.transform)
            {
                if (child.CompareTag("UI"))
                {
                    Destroy(child.gameObject); 
                }
                else Debug.LogError("No object found inside the WeaponAttachmentSystem with tag 'UI'");
            }         
        }
    }
}