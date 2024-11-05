using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject _maineMenu;
    [SerializeField] private GameObject _singlepalyerMenu;
    [SerializeField] private GameObject _multiplayerMenu;
    [SerializeField] private GameObject _settingsMenu;
    private GameObject _lastMenu;
    private GameObject _currentMenu;

    [Header ("Buttons")]
    [SerializeField] private Button _singlePlayerButton;
    [SerializeField] private Button _multiPlayerButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private GameObject _backButton;

    [Header("Missions")]
    [SerializeField] private GameObject _okButton;
    [SerializeField] private GameObject _missionBriefs;

    [Header ("Cursor")]
    [SerializeField] private Texture2D _cursorTexture;
    private Vector2 _cursorHotspot;

    private void Start()
    {
        _cursorHotspot = new Vector2(_cursorTexture.width/2, _cursorTexture.height/2);
        //Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.ForceSoftware);

        _maineMenu.SetActive (true);
        _singlepalyerMenu.SetActive (false);
        _multiplayerMenu.SetActive (false);
        _settingsMenu.SetActive (false);

        _currentMenu = _maineMenu;
        _lastMenu = _maineMenu;
    }

    public void OpenSinglePlayerMenu()
    {
        _maineMenu.SetActive(false);
        _singlepalyerMenu.SetActive(true);
        _backButton.SetActive(true);
        _currentMenu = _singlepalyerMenu;
        _lastMenu = _maineMenu;
    }
    public void OpenMultiPlayerMenu()
    {
        _maineMenu.SetActive(false);
        _multiplayerMenu.SetActive(true);
        _backButton.SetActive(true);
        _currentMenu = _multiplayerMenu;
        _lastMenu = _maineMenu;
    }
    public void OpenSettingsMenu() 
    {
        _maineMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _backButton.SetActive(true);
        _currentMenu = _settingsMenu;
        _lastMenu = _maineMenu;
    }
    public void HandleBackButton()
    {
        _currentMenu.SetActive(false);
        _backButton.SetActive(false);
        _missionBriefs.SetActive(false);
        _lastMenu.SetActive(true);
        _currentMenu = _lastMenu;
    }
    public void DisplayBriefs()
    {
        _missionBriefs.SetActive(true);
        _okButton.SetActive(true);
    }
    public void HandleOkButton()
    {    
        SceneManager.LoadScene(1);
    }
}
