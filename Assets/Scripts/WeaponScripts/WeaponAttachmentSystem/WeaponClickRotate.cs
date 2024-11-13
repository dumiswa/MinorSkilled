using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClickRotate : MonoBehaviour
{
    private Transform _weaponTransform;
    private Vector3 _lastMousePosition;


    private void Awake()
        => _weaponTransform = transform;
    

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = _lastMousePosition - Input.mousePosition;

            // Fix huge jumps when Unity loses focus
            mouseDelta.y = Mathf.Clamp(mouseDelta.y, -10, +10);
            mouseDelta.x = Mathf.Clamp(mouseDelta.x, -10, +10);

            float rotateSpeed = .2f;

            _weaponTransform.localEulerAngles += new Vector3(mouseDelta.y, mouseDelta.x, 0f) * rotateSpeed;

            float rotationXMin = -1f;
            float rotationXMax = +2f;

            float localEulerAnglesX = _weaponTransform.localEulerAngles.x;
            if (localEulerAnglesX > 180)            
                localEulerAnglesX -= 360f;
            
            float rotationX = Mathf.Clamp(localEulerAnglesX, rotationXMin, rotationXMax);

            _weaponTransform.localEulerAngles = new Vector3(rotationX, _weaponTransform.localEulerAngles.y, _weaponTransform.localEulerAngles.z);
        }

        _lastMousePosition = Input.mousePosition;
    }
}