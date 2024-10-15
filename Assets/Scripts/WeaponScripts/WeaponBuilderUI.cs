using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WeaponBuilderUI : MonoBehaviour
{
    public WeaponBuilder weaponBuilder;

    public GameObject handguardMenu;
    public GameObject muzzleMenu;
    public GameObject stockMenu;
    public GameObject triggerMenu;
    public GameObject scopeMenu;
    public GameObject frontGripMenu;
    public GameObject backGripMenu;
    public GameObject magazineMenu;

    public Button handguardButton;
    public Button muzzleButton;
    public Button stockButton;
    public Button triggerButton;
    public Button scopeButton;
    public Button frontGripButton;
    public Button backGripButton;
    public Button magazineButton;

    void Start()
    {
        handguardButton.onClick.AddListener(() => OpenMenu(handguardMenu));
        muzzleButton.onClick.AddListener(() => OpenMenu(muzzleMenu));
        stockButton.onClick.AddListener(() => OpenMenu(stockMenu));
        triggerButton.onClick.AddListener(() => OpenMenu(triggerMenu));
        scopeButton.onClick.AddListener(() => OpenMenu(scopeMenu));
        frontGripButton.onClick.AddListener(() => OpenMenu(frontGripMenu));
        backGripButton.onClick.AddListener(() => OpenMenu(backGripMenu));
        magazineButton.onClick.AddListener(() => OpenMenu(magazineMenu));
    }

    private void OpenMenu(GameObject menu)
    {
        handguardMenu.SetActive(false);
        muzzleMenu.SetActive(false);
        stockMenu.SetActive(false);
        triggerMenu.SetActive(false);
        scopeMenu.SetActive(false);
        frontGripMenu.SetActive(false);
        backGripMenu.SetActive(false);
        magazineMenu.SetActive(false);

        menu.SetActive(true);
    }
    private void CloseMenu()
    {
        handguardMenu.SetActive(false);
        muzzleMenu.SetActive(false);
        stockMenu.SetActive(false);
        triggerMenu.SetActive(false);
        scopeMenu.SetActive(false);
        frontGripMenu.SetActive(false);
        backGripMenu.SetActive(false);
        magazineMenu.SetActive(false);
    }

    public void SelectPart(GameObject selectedPart)
    {
        weaponBuilder.ChangeWeaponPart(selectedPart);
        CloseMenu();
    }

    public void StartGame()
    {
        weaponBuilder.SaveModifiedGun();
        SceneManager.LoadScene(1);
    }
}
