using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Animation Curve")]
    [SerializeField]
    private AnimationCurve _rotationX = new(
        new Keyframe(0.0f, 0.0f),
        new Keyframe(0.25f, -0.05f), // Softer kick up
        new Keyframe(0.15f, 0.0f)    // Longer return to zero
    );

    [SerializeField]
    private AnimationCurve _rotationY = new(
        new Keyframe(0.0f, 0.0f),
        new Keyframe(0.15f, -0.05f), // Softer kick up
        new Keyframe(0.3f, 0.0f)    // Longer return to zero
    );

    [SerializeField]
    private AnimationCurve _rotationZ = new(
        new Keyframe(0.0f, 0.0f),
        new Keyframe(0.35f, -0.15f), // Softer kick up
        new Keyframe(0.2f, 0.0f)    // Longer return to zero
    );

    [SerializeField]
    private AnimationCurve _positionX = new(
       new Keyframe(0.0f, 0.0f),
       new Keyframe(0.15f, -0.07f), // Softer kick up
       new Keyframe(0.3f, 0.0f)    // Longer return to zero
    );

    [SerializeField]
    private AnimationCurve _positionY = new(
       new Keyframe(0.0f, 0.0f),
       new Keyframe(0.15f, -0.05f), // Softer kick up
       new Keyframe(0.3f, 0.0f)    // Longer return to zero
    );

    [SerializeField]
    private AnimationCurve _positionZ = new(
        new Keyframe(0.0f, 0.0f),
        new Keyframe(0.15f, -0.05f), // Softer kick up
        new Keyframe(0.3f, 0.0f)    // Longer return to zero
    );

    [SerializeField] private float AnimationcurveTime = 0;
    [SerializeField] private float duration = 0.35f;

    private float _timePassed;
    private Coroutine _recoilCoroutine;

    private IEnumerator StartRecoil()
    {
        _timePassed = 0;
        AnimationcurveTime = 0;

        Quaternion initialRotation = transform.localRotation;
        Vector3 initialPosition = transform.localPosition;

        // Recoil phase
        while (_timePassed < duration)
        {
            _timePassed += Time.deltaTime;
            AnimationcurveTime = _timePassed / duration;

            float easedTime = Mathf.SmoothStep(0, 1, AnimationcurveTime);

            transform.localRotation = Quaternion.Lerp(initialRotation, initialRotation * Quaternion.Euler(CalculateNextRotation()), easedTime);
            transform.localPosition = Vector3.Lerp(initialPosition, initialPosition + CalculateNextPosition(), easedTime);

            yield return null;
        }

        // Recovery phase (smooth return to original state)
        _timePassed = 0;
        while (_timePassed < duration)
        {
            _timePassed += Time.deltaTime;
            float recoveryTime = Mathf.SmoothStep(1, 0, _timePassed / duration);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, recoveryTime);
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, recoveryTime);

            yield return null;
        }
    }

    private Vector3 CurrentRotation, CurrentPosition;
    public Vector3 CalculateNextRotation()
    {
        float aimMultiplier = 1.0f;
        if (GetComponent<WeaponADS>().IsAiming)
        {
            aimMultiplier = 0.5f; // Reduce recoil by 50% when aiming
        }

        float Rotationx = _rotationX.Evaluate(AnimationcurveTime) * aimMultiplier;
        float Rotationy = _rotationY.Evaluate(AnimationcurveTime) * aimMultiplier;
        float Rotationz = _rotationZ.Evaluate(AnimationcurveTime) * aimMultiplier;

        CurrentRotation = new Vector3(Rotationx, Rotationy, Rotationz);

        return CurrentRotation;
    }
    public Vector3 CalculateNextPosition()
    {
        float aimMultiplier = 1.0f;
        if (GetComponent<WeaponADS>().IsAiming)
        {
            aimMultiplier = 0.5f; // Reduce recoil by 50% when aiming
        }

        float Positionx = _positionX.Evaluate(AnimationcurveTime) * aimMultiplier;
        float Positiony = _positionY.Evaluate(AnimationcurveTime) * aimMultiplier;
        float Positionz = _positionZ.Evaluate(AnimationcurveTime) * aimMultiplier;

        CurrentPosition = new Vector3(Positionx, Positiony, Positionz);
        return CurrentPosition;
    }
    public void ApplyRecoil()
    {
        if (_recoilCoroutine != null)
        {
            StopCoroutine(_recoilCoroutine);
        }
        _recoilCoroutine = StartCoroutine(StartRecoil());
    }
}