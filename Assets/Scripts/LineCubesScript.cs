using UnityEngine;

public class LineOfCubes : MonoBehaviour
{
    public GameObject Crate_01; // Assign your cube prefab in the inspector
    public int numberOfCubes = 10; // How many cubes you want in the line
    public float spacing = 2.0f; // Spacing between each cube
    public float xPosition = 12.49f;
    public float yPosition = -1.76f;
    void Start()
    {
        CreateLineOfCubes();
    }

    void CreateLineOfCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            // Calculate the position for the new cube
            Vector3 position = new Vector3(xPosition , yPosition, (i-5) * spacing);

            // Instantiate the cube at the calculated position
            Instantiate(Crate_01, position, Quaternion.identity);
        }
    }
}
