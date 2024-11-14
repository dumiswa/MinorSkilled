using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class WeaponComplete : MonoBehaviour
{
    public class AttachedWeaponPart
    {
        public WeaponBody.PartTypeAttachPoint partTypeAttachPoint;
        public WeaponPartSO weaponPartSO;
        public Transform spawnedTransform;
    }


    [SerializeField] private List<WeaponPartSO> defaultWeaponPartSOList;


    private WeaponBody weaponBody;
    private Dictionary<WeaponPartSO.PartType, AttachedWeaponPart> attachedWeaponPartDic;


    private void Awake()
    {
        weaponBody = GetComponent<WeaponBody>();

        attachedWeaponPartDic = new Dictionary<WeaponPartSO.PartType, AttachedWeaponPart>();

        foreach (WeaponBody.PartTypeAttachPoint partTypeAttachPoint in weaponBody.GetPartTypeAttachPointList())
        {
            attachedWeaponPartDic[partTypeAttachPoint.partType] = new AttachedWeaponPart
            {
                partTypeAttachPoint = partTypeAttachPoint,
            };
        }

        foreach (WeaponPartSO weaponPartSO in defaultWeaponPartSOList)
        {
            SetPart(weaponPartSO);
        }
    }



    public void SetPart(WeaponPartSO weaponPartSO)
    {
        // Destroy currently attached part
        if (attachedWeaponPartDic[weaponPartSO.partType].spawnedTransform != null)
        {
            Destroy(attachedWeaponPartDic[weaponPartSO.partType].spawnedTransform.gameObject);
        }

        // Ensure proper instantiation
        Transform spawnedPartTransform = Instantiate(weaponPartSO.prefab).transform;
        AttachedWeaponPart attachedWeaponPart = attachedWeaponPartDic[weaponPartSO.partType];
        attachedWeaponPart.spawnedTransform = spawnedPartTransform;

        Transform attachPointTransform = attachedWeaponPart.partTypeAttachPoint.attachPointTransform;
        spawnedPartTransform.SetParent(attachPointTransform, false);
        //spawnedPartTransform.localEulerAngles = Vector3.zero;
        //spawnedPartTransform.localRotation = Quaternion.identity;
        //spawnedPartTransform.localPosition = Vector3.zero;
        //spawnedPartTransform.localScale = Vector3.one;

        attachedWeaponPart.weaponPartSO = weaponPartSO;
        attachedWeaponPartDic[weaponPartSO.partType] = attachedWeaponPart;
        // Safe casting for HandGuard
        if (weaponPartSO is HandGuardWeaponPartSO handGuardPartSO)    
            AdjustMuzzleOffset(handGuardPartSO);    
    }
    private void AdjustMuzzleOffset(HandGuardWeaponPartSO handguardWeaponPartSO)
    {
        AttachedWeaponPart handguardPartTypeAttachedWeaponPart = attachedWeaponPartDic[WeaponPartSO.PartType.HandGuard];
        AttachedWeaponPart muzzlePartTypeAttachedWeaponPart = attachedWeaponPartDic[WeaponPartSO.PartType.Muzzle];

        muzzlePartTypeAttachedWeaponPart.partTypeAttachPoint.attachPointTransform.position =
            handguardPartTypeAttachedWeaponPart.partTypeAttachPoint.attachPointTransform.position +
            handguardPartTypeAttachedWeaponPart.partTypeAttachPoint.attachPointTransform.forward * handguardWeaponPartSO.muzzleOffset;
    }



    public WeaponPartSO GetWeaponPartSO(WeaponPartSO.PartType partType)
    {
        AttachedWeaponPart attachedWeaponPart = attachedWeaponPartDic[partType];
        return attachedWeaponPart.weaponPartSO;
    }
    public List<WeaponPartSO.PartType> GetWeaponPartTypeList()
    {
        return new List<WeaponPartSO.PartType>(attachedWeaponPartDic.Keys);
    }
    public WeaponBodySO GetWeaponBodySO()
    {
        return weaponBody.GetWeaponBodySO();
    }



    public string Save()
    {
        List<WeaponPartSO> weaponPartSOList = new List<WeaponPartSO>();
        foreach (WeaponPartSO.PartType partType in attachedWeaponPartDic.Keys)
        {
            if (attachedWeaponPartDic[partType].weaponPartSO != null)
            {
                weaponPartSOList.Add(attachedWeaponPartDic[partType].weaponPartSO);
            }
        }

        SaveObject saveObject = new SaveObject()
        {
            weaponBodySO = GetWeaponBodySO(),
            weaponPartSOList = weaponPartSOList,
        };

        string json = JsonUtility.ToJson(saveObject);

        return json;
    }
    public void Load(string json)
    {
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);

        foreach (WeaponPartSO weaponPartSO in saveObject.weaponPartSOList)
        {
            SetPart(weaponPartSO);
        }
    }


    [Serializable]
    public class SaveObject
    {
        public WeaponBodySO weaponBodySO;
        public List<WeaponPartSO> weaponPartSOList;
    }


    public static WeaponComplete LoadSpawnWeaponComplete(string json, bool spawnUI)
    {
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);

        Transform parentTransform = WeaponAttachmentSystem.Instance?.transform;
        Transform weaponCompleteTransform = Instantiate(saveObject.weaponBodySO.prefab, parentTransform);

        WeaponComplete weaponComplete = weaponCompleteTransform.GetComponent<WeaponComplete>();

        weaponComplete.Load(json);

        Transform weaponModel = null;
        foreach (Transform child in weaponCompleteTransform.GetComponentInChildren<Transform>(true))
        {
            if (child.CompareTag("Weapon"))
            {
                weaponModel = child;
                weaponModel.localPosition = new Vector3(0, 0.62f, -4.7f);
                weaponModel.localRotation = Quaternion.Euler(0, -90, 0);
                break;
            }
        }

        if (spawnUI)
        {
            // Instantiate UI and ensure its transform is correctly reset
            Transform uiTransform = Instantiate(saveObject.weaponBodySO.prefabUI, weaponCompleteTransform);
            uiTransform.localPosition = Vector3.zero;
            uiTransform.localRotation = Quaternion.identity;
            uiTransform.localScale = Vector3.one;

            // Ensure children of the UI are correctly positioned
            foreach (Transform child in uiTransform)
            {
                child.localPosition = child.localPosition; // Ensure positions are respected
                child.localRotation = Quaternion.identity; // Reset rotation
            }
        }

        return weaponComplete;
    }
}

