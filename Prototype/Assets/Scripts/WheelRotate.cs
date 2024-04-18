using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    public float rotationSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // Вращение объекта по оси Z
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}