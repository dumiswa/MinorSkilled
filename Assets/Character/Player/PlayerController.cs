using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private Vector3 _direction;
    private Rigidbody _rb;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _maxVelocity = 10f;

    [SerializeField] private Transform _cameraTransform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 move = 
            _cameraTransform.forward * _input.y + _cameraTransform.right * _input.x;
        move.y = 0f;
        _rb.AddForce(move.normalized * _moveSpeed, ForceMode.VelocityChange);
        //_rb.AddForce(-_direction * _moveSpeed * 100, ForceMode.Acceleration);
        if (_rb.velocity.magnitude > _maxVelocity)
        {
            _rb.velocity = _rb.velocity.normalized * _maxVelocity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();     
    }
}
