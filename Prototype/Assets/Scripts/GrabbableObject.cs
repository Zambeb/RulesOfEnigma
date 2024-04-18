using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Проверяем, прикреплен ли объект к персонажу (transform.parent не равен null)
        if (transform.parent != null)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            // Применяем движение с помощью Rigidbody.AddForce
            rb.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }


}