using UnityEngine;

public class CarControllerWASD : MonoBehaviour
{
    public float acceleration = 50f; // How quickly the car accelerates
    public float maxSpeed = 10000f; // The maximum speed of the car
    public float rotationSpeed = 100f;
    public float decelerationFactor = 0.9f; // Deceleration factor when hitting an object
    private float currentSpeed = 10f; // Current speed of the car

    void Update()
    {
        // Accelerate or decelerate
        float verticalInput = Input.GetAxis("VerticalWASD");
        if (verticalInput != 0)
        {
            // Increase current speed based on acceleration, without exceeding maxSpeed
            currentSpeed += verticalInput * acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
        else
        {
            // Gradually reduce speed to 0 if no input is detected
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * acceleration);
        }

        // Rotate the car
        float rotation = Input.GetAxis("HorizontalWASD") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        // Move the car forward or backward based on current speed
        transform.Translate(0, 0, currentSpeed * Time.deltaTime);
    }

}
