using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentSystem : MonoBehaviour
{
    public static WeaponAttachmentSystem Instance { get; private set; }

    public event EventHandler OnPartChanged;

    [SerializeField] private WeaponBodyListSO _weaponBodyListSO;
    [SerializeField] private WeaponBodySO _weaponBodySO;
    [SerializeField] private WeaponComplete _weaponComplete;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetWeaponBody(_weaponBodySO);
    }


    public void SetWeaponBody(WeaponBodySO weaponBodySO)
    {
        Vector3 previousEularAngles = Vector3.zero;

        if(_weaponComplete != null)
        {
            previousEularAngles = _weaponComplete.transform.eulerAngles;
            Destroy(_weaponComplete.gameObject);
        }

        this._weaponBodySO = weaponBodySO;

        Transform weaponBodyTransform = Instantiate(_weaponBodySO.prefab);
        weaponBodyTransform.eulerAngles = previousEularAngles;
        _weaponComplete = weaponBodyTransform.GetComponent<WeaponComplete>();

        Instantiate(weaponBodySO.prefabUI, weaponBodyTransform);
    }

    public int GetPartIndex(WeaponPartSO.PartType partType)
    {
        WeaponPartSO attachedWeaponPartSO = _weaponComplete.GetWeaponPartSO(partType);
        if (attachedWeaponPartSO == null)
        {
            return 0;
        }
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
            {
                ChangePart(partType);
            }
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
    {
        _weaponComplete.transform.eulerAngles = Vector3.zero;
    }
}