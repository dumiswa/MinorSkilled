using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WeaponBuilderUI : MonoBehaviour
{
    public WeaponBuilder WeaponBuilder;

    [SerializeField] private Button _scopeButton;
    [SerializeField] private Button _muzzleButton;
    [SerializeField] private Button _backGripButton;
    [SerializeField] private Button _frontGripButton;
    [SerializeField] private Button _handguardButton;
    [SerializeField] private Button _magazineButton;
    [SerializeField] private Button _stockButton;
    [SerializeField] private Button _triggerButton;

    [SerializeField] private GameObject _partsOptionPanel;
    [SerializeField] private Button _partsOptionButtonPrefab;

    private void Start()
    {
        _scopeButton.onClick.AddListener(() => ShowPartOptions(PartType.Scope));
        _muzzleButton.onClick.AddListener(() => ShowPartOptions(PartType.Muzzle));
        _backGripButton.onClick.AddListener(() => ShowPartOptions(PartType.BackGrip));
        _frontGripButton.onClick.AddListener(() => ShowPartOptions(PartType.FrontGrip));
        _handguardButton.onClick.AddListener(() => ShowPartOptions(PartType.Handguard));
        _magazineButton.onClick.AddListener(() => ShowPartOptions(PartType.Magazine));
        _stockButton.onClick.AddListener(() => ShowPartOptions(PartType.Stock));
        _triggerButton.onClick.AddListener(() => ShowPartOptions(PartType.Trigger));
    }

    public void ShowPartOptions(PartType partType)
    {
        foreach (Transform child in _partsOptionPanel.transform)
            Destroy(child.gameObject);

        WeaponPart[] availableParts = GetAvailableParts(partType);

        foreach (var part in availableParts)
        {
            Button optionButton = Instantiate(_partsOptionButtonPrefab, _partsOptionPanel.transform);
            var textComponent = optionButton.GetComponentInChildren<TextMeshProUGUI>();

            textComponent.text = part.name;
            optionButton.onClick.AddListener(() => SelectPart(partType, part));
        }

        _partsOptionPanel.SetActive(true);
    }

    private WeaponPart[] GetAvailableParts(PartType partType)
    {
        return WeaponBuilder.GetAvailablePartsByType(partType);
    }

    public void SelectPart(PartType partType, WeaponPart selectedPart)
    {
        //Debug.Log($"Selected part: {selectedPart.name} for type: {partType}");

        switch (partType)
        {
            case PartType.Scope:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedScope.PartModel, "Scope");
                break;
            case PartType.Muzzle:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedMuzzle.PartModel, "Muzzle");
                break;
            case PartType.BackGrip:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedBackGrip.PartModel, "BackGrip");
                break;
            case PartType.FrontGrip:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedFrontGrip.PartModel, "FrontGrip");
                break;
            case PartType.Handguard:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedHandguard.PartModel, "Handguard");
                break;
            case PartType.Trigger:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedTrigger.PartModel, "Trigger");
                break;
            case PartType.Stock:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedStock.PartModel, "Stock");
                break;
            case PartType.Magazine:
                WeaponBuilder.ReplacePart(selectedPart.PartModel, ref WeaponBuilder.SelectedMagazine.PartModel, "Magazine");
                break;
        }

        _partsOptionPanel.SetActive(false);
    }
}
