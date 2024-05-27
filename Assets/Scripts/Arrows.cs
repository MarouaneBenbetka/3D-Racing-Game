using UnityEngine;

public class CarControllerArrows : MonoBehaviour
{
    public float speed = 100f;
    public float rotationSpeed = 100f;

    void Update()
    {
        float translation = Input.GetAxis("VerticalArrows") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("HorizontalArrows") * rotationSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}
