using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References for Cameras and Arms Rig")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cinemachineTransform;
    [SerializeField] private Transform _armsTransform;

    [Header("Movement Values")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxVelocity;

    [Header("Bob and Tilt Values")]
    [SerializeField] private float _bobFrequency;
    [SerializeField] private float _bobAmplitude;
    [SerializeField] private float _forwardBobReduction;
    [SerializeField] private float _tiltAmount;
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _walkingTiltReduction;

    [Header("Procedural Camera Shake")]
    [SerializeField] private float _cameraShakeIntensityX;
    [SerializeField] private float _cameraShakeIntensityY;
    [SerializeField] private float _cameraShakeFrequency;
    [SerializeField] private float _shakeLerpSpeed;

    private Vector2 _input;
    private Rigidbody _rb;

    private Vector3 _armsDefaultPosition;
    private Quaternion _armsDefaultRotation;
   
    private Vector3 _currentTargetPosition;
    private Quaternion _currentTargetRotation;

    private Vector3 _cinemachineDefaultPosition;
    private Vector3 _cinemachineCameraShakeOffset;

    private bool _isMoving = false;
    private bool _wasMovingLastFrame = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cinemachineDefaultPosition = _cinemachineTransform.localPosition;
    }

    private void FixedUpdate()
    {
        Vector3 move =
            _cameraTransform.forward * _input.y + _cameraTransform.right * _input.x;
        move.y = 0f;
        _rb.AddForce(move.normalized * _moveSpeed, ForceMode.VelocityChange);

        if (_rb.velocity.magnitude > _maxVelocity)
            _rb.velocity = _rb.velocity.normalized * _maxVelocity;  

        UpdateArmPositionAndTilt();
        ApplyCameraShake();
    }

    public void SetArmsRig(Transform armsRig)
    {
        _armsTransform = armsRig;

        // Store the default position and rotation for use in bobbing and tilting
        _armsDefaultPosition = _armsTransform.localPosition;
        _armsDefaultRotation = _armsTransform.localRotation;
        _currentTargetPosition = _armsDefaultPosition;
        _currentTargetRotation = _armsDefaultRotation;
    }

    private void UpdateArmPositionAndTilt()
    {
        if (_input.magnitude > 0.1f) 
        {
            _isMoving = true;

            float adjustedBobAmplitude = (_input.y != 0) ? _bobAmplitude * _forwardBobReduction : _bobAmplitude;
            float bobOffset = Mathf.Sin(Time.time * _bobFrequency) * adjustedBobAmplitude;
            float tiltDirection = _input.x;

            if (_input.y != 0)           
                tiltDirection += Mathf.Sin(Time.time * _bobFrequency) * _walkingTiltReduction;
            
            Quaternion targetTilt = Quaternion.Euler(0, 0, -tiltDirection * _tiltAmount);
            _currentTargetPosition = _armsDefaultPosition + new Vector3(0, bobOffset, 0);
            _currentTargetRotation = _armsDefaultRotation * targetTilt;
        }
        else if (_isMoving) 
        {
            _isMoving = false;

            _currentTargetPosition = _armsDefaultPosition;
            _currentTargetRotation = _armsDefaultRotation;
        }

        _armsTransform.localPosition = Vector3.Lerp(_armsTransform.localPosition, _currentTargetPosition, Time.deltaTime * _lerpSpeed);
        _armsTransform.localRotation = Quaternion.Lerp(_armsTransform.localRotation, _currentTargetRotation, Time.deltaTime * _lerpSpeed);

        _wasMovingLastFrame = _isMoving;
    }

    private void ApplyCameraShake()
    {
        if (_isMoving)
        {
            float shakeX = Mathf.PerlinNoise(Time.time * _cameraShakeFrequency, 0) * _cameraShakeIntensityX - (_cameraShakeIntensityX / 2);
            float shakeY = Mathf.PerlinNoise(0, Time.time * _cameraShakeFrequency) * _cameraShakeIntensityY + (_cameraShakeIntensityY / 2);
            _cinemachineCameraShakeOffset = 
                Vector3.Lerp(_cinemachineCameraShakeOffset, new Vector3(shakeX, shakeY, 0), Time.deltaTime * _shakeLerpSpeed);
        }
        else _cinemachineCameraShakeOffset = Vector3.Lerp(_cinemachineCameraShakeOffset, Vector3.zero, Time.deltaTime * _shakeLerpSpeed);

        _cinemachineTransform.localPosition = _cinemachineDefaultPosition + _cinemachineCameraShakeOffset;
    }

    public void Move(InputAction.CallbackContext context)
        => _input = context.ReadValue<Vector2>();
}
