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
        if (transform.parent != null)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            rb.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}