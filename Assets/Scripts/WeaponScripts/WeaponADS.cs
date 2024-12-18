using UnityEngine;

public class WeaponADS : MonoBehaviour
{
    [Header("Weapon / Camera")]
    public Transform _weaponADSLayer;
    [SerializeField] private Camera _camera;

    [Header("Variables")]
    public float smoothTime;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public bool IsAiming = false;

    [Header("Keys")]
    public KeyCode ADSKey = KeyCode.Mouse1;

    private Vector3 originalWeaponPosition;


    private void Start()
    {
        _camera = GetComponentInParent<Camera>();
        originalWeaponPosition = _weaponADSLayer.localPosition;


        UpdateAiming(false);
    }

    private void Update()
    {
        MyInput();
        HandleAiming();
    }

    private void HandleAiming()
    {
        if (IsAiming)
        {
            // Calculate the target screen position at the center of the screen
            Vector3 targetScreenPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

            // Convert the screen position to a world position based on the weapon's distance from the camera
            float distanceFromCamera = Vector3.Distance(_weaponADSLayer.position, _camera.transform.position);
            Vector3 targetWorldPosition = _camera.ScreenToWorldPoint(new Vector3(targetScreenPosition.x, targetScreenPosition.y, distanceFromCamera));

            // Convert the world position to a local position relative to the weapon
            Vector3 targetLocalPosition = _weaponADSLayer.parent.InverseTransformPoint(targetWorldPosition);

            // Apply the specified offsets
            targetLocalPosition += new Vector3(offsetX, offsetY, offsetZ);
            targetLocalPosition.z = offsetZ;
            // Lerp the weapon position to match the target position
            _weaponADSLayer.localPosition = Vector3.Lerp(_weaponADSLayer.localPosition, targetLocalPosition, Time.deltaTime * smoothTime);
        }
        else
        {
            _weaponADSLayer.localPosition = Vector3.Lerp(_weaponADSLayer.localPosition, originalWeaponPosition, Time.deltaTime * smoothTime);
        }
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(ADSKey))
        {
            UpdateAiming(true);
        }
        if (Input.GetKeyUp(ADSKey))
        {
            UpdateAiming(false);
        }
    }

    private void UpdateAiming(bool Aiming)
    {
        IsAiming = Aiming;
    }
}
