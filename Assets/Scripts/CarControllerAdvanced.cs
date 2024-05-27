using UnityEngine;
using System;
using System.Collections.Generic;

public class CarControllerAdvanced : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    public ControlMode control;
    public float maxAcceleration = 20.0f;
    public float brakeAcceleration = 80.0f;
    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;
    public Vector3 _centerOfMass;
    public List<Wheel> wheels;
    public string inputType = "arrows";

    // Audio fields

    public AudioSource accelerationSource;
    public AudioSource decelerationSource;

    float moveInput;
    float steerInput;
    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
        //WheelEffects();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
        //HandleAudio(); // Handle audio playback
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            if (inputType == "arrows")
            {
                moveInput = Input.GetAxis("VerticalArrows");
                steerInput = Input.GetAxis("HorizontalArrows");
            }
            else
            {
                moveInput = Input.GetAxis("VerticalWASD");
                steerInput = Input.GetAxis("HorizontalWASD");
            }
        }
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 200 * maxAcceleration * Time.deltaTime;
        }
        if (moveInput > 0)
        {
            if (!accelerationSource.isPlaying)
                accelerationSource.Play();
        }
        else if (moveInput < 0)
        {
            accelerationSource.Stop();
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.3f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.smokeParticle != null && Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded && carRb.velocity.magnitude >= 10.0f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);
            }
            else if (wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>() != null)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    private float previousVelocityMagnitude = 0.0f; // Store the car's velocity magnitude from the last frame.

    // Handle audio playback based on acceleration and deceleration
    void HandleAudio()
    {
        float currentVelocityMagnitude = carRb.velocity.magnitude; // Get current velocity magnitude
        bool isAccelerating = moveInput > 0; // Accelerating when moveInput is positive
        bool isDecelerating = (previousVelocityMagnitude > currentVelocityMagnitude) && Mathf.Abs(moveInput) < 0.1f; // Decelerating when current velocity is less than previous velocity and moveInput is near zero

        // Play acceleration sound when the car is accelerating.
        if (isAccelerating)
        {
            if (!accelerationSource.isPlaying)
            {
                accelerationSource.Play();
                decelerationSource.Stop(); // Ensure deceleration sound stops if it was playing.
            }
        }
        else if (isDecelerating) // Play deceleration sound when the car is decelerating.
        {
            if (!decelerationSource.isPlaying)
            {
                decelerationSource.Play();
                accelerationSource.Stop(); // Ensure acceleration sound stops if it was playing.
            }
        }
        else
        {
            // Optional: Handle cases for when the car is neither clearly accelerating nor decelerating.
            // This could be useful for playing idle sounds or ensuring sounds stop in certain conditions.
            if (accelerationSource.isPlaying)
            {
                accelerationSource.Stop();
            }
            if (decelerationSource.isPlaying)
            {
                decelerationSource.Stop();
            }
        }

        // Update previousVelocityMagnitude for the next frame's comparison.
        previousVelocityMagnitude = currentVelocityMagnitude;
    }
}

