using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraBack : MonoBehaviour
{
    

    public Transform objectToMove; // Объект, который нужно передвигать
    public float moveSpeed = 1.0f; // Скорость передвижения
    public float moveDistance = 10.0f; // Расстояние для передвижения
    public Vector3 moveDirection = Vector3.forward; 
    
    public Transform cameraToRotate; // Камера, которую вы хотите поворачивать
    public float rotationSpeed = 10.0f; // Скорость вращения
    public float rotationAngle = 40.0f; // Угол вращения

    public bool playerInTrigger = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float currentRotationAngle = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = objectToMove.position;
        //initialRotation = cameraToRotate.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
        MoveBack();
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            MoveObject();
           /* if (currentRotationAngle < currentRotationAngle)
            {
                RotateCamera();
            }
            */
        }
    }

    private void MoveObject()
    {
        

        // Проверяем, достиг ли объект нужного расстояния
        if (Vector3.Distance(transform.position, objectToMove.position) >= moveDistance)
        {
            // Если достиг, можно выполнить дополнительные действия или остановить движение
            // Например, можно выключить скрипт или отключить коллайдер триггера
            // gameObject.SetActive(false);
            // GetComponent<Collider>().enabled = false;

            // В данном случае, просто выводим сообщение в консоль
            Debug.Log("Object reached the destination.");
        }
        else
        {
            float step = moveSpeed * Time.deltaTime;

            // Передвигаем объект в заданном направлении
            objectToMove.Translate(moveDirection * step);

        }
    }
    
    private void MoveBack()
    {
        objectToMove.position = new Vector3(objectToMove.position.x, objectToMove.position.y, initialPosition.z);
    }
    
    private void RotateCamera()
    {
        float step = rotationSpeed * Time.deltaTime;

        // Поворачиваем камеру по оси X
        currentRotationAngle += rotationSpeed * Time.deltaTime;
        if (currentRotationAngle < rotationAngle)
        {
            cameraToRotate.Rotate(Vector3.right, rotationAngle * step);
        }
    }

    private void RotateCameraBack()
    {
        // Возвращаем камеру в изначальное положение
        cameraToRotate.rotation = Quaternion.Slerp(cameraToRotate.rotation, initialRotation, Time.deltaTime * rotationSpeed);
    }
    
}
