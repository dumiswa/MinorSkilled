using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Stores current frame's mouse input in the cache, calculates the average over the last 3 frames and applies the smoothed rotation (backend smoothing)
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    private Vector2 _mouseInput;
    private float _pitch;

    private static int _maxRotCache = 3;
    private float[] _rotArrayHor = new float[_maxRotCache];
    private float[] _rotArrayVer = new float[_maxRotCache];
    private int _rotCacheIndex = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        /*transform.Rotate(Vector3.up, _mouseInput.x * _sensitivity * Time.deltaTime);

        _pitch -= _mouseInput.y * _sensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        transform.localEulerAngles = new Vector3(_pitch, transform.localEulerAngles.y, 0f);*/

        // Store current frame's mouse input
        _rotArrayHor[_rotCacheIndex] = _mouseInput.x * _sensitivity * Time.deltaTime;
        _rotArrayVer[_rotCacheIndex] = _mouseInput.y * _sensitivity * Time.deltaTime;

        // Calculate the average of the last few frames
        float avgHor = GetAverageRotateHor();
        float avgVer = GetAverageRotateVer();

        // Apply the smoothed horizontal rotation
        transform.Rotate(Vector3.up, avgHor);

        // Apply the smoothed vertical rotation
        _pitch -= avgVer;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f); // Clamp the pitch to avoid flipping the camera
        transform.localEulerAngles = new Vector3(_pitch, transform.localEulerAngles.y, 0f);

        // Move to the next cache index
        IncreaseRotCacheIndex();
    }

    private float GetAverageRotateHor() // Averages horizontal rotation
    {
        float sum = 0f;
        for (int i = 0; i < _rotArrayHor.Length; i++)
            sum += _rotArrayHor[i];
        return sum / _rotArrayHor.Length;
    }
    private float GetAverageRotateVer() // Averages vertical rotation
    {
        float sum = 0f;
        for (int i = 0; i < _rotArrayVer.Length; i++)
            sum += _rotArrayVer[i];
        return sum / _rotArrayVer.Length;
    }
    private void IncreaseRotCacheIndex() // Moves to the next cache index, wrapping around if needed
    {
        _rotCacheIndex++;
        _rotCacheIndex %= _maxRotCache;
    }

    public void OnMouseMove(InputAction.CallbackContext context)
        =>_mouseInput = context.ReadValue<Vector2>(); 
    
}
