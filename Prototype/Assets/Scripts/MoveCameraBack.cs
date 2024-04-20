using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraBack : MonoBehaviour
{
    

    public Transform objectToMove; 
    public float moveSpeed = 1.0f; 
    public float moveDistance = 10.0f;
    public Vector3 moveDirection = Vector3.forward; 
    
    public Transform cameraToRotate; 
    public float rotationSpeed = 10.0f;
    public float rotationAngle = 40.0f;

    public bool playerInTrigger = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float currentRotationAngle = 0.0f;
    
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
        if (Vector3.Distance(transform.position, objectToMove.position) >= moveDistance)
        {
            // gameObject.SetActive(false);
            // GetComponent<Collider>().enabled = false;
            
            Debug.Log("Object reached the destination.");
        }
        else
        {
            float step = moveSpeed * Time.deltaTime;
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
        
        currentRotationAngle += rotationSpeed * Time.deltaTime;
        if (currentRotationAngle < rotationAngle)
        {
            cameraToRotate.Rotate(Vector3.right, rotationAngle * step);
        }
    }

    private void RotateCameraBack()
    {
        cameraToRotate.rotation = Quaternion.Slerp(cameraToRotate.rotation, initialRotation, Time.deltaTime * rotationSpeed);
    }
    
}
