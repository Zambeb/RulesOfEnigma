using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRise : MonoBehaviour
{
    public Transform pipe;
    public Transform trigger; 

    public float riseSpeed = 3.0f;
    public float fallSpeed = 0.5f;
    public float riseDistance = 5.0f; 
    private Vector3 initialPosition;

    public GameObject sparks;

    public AudioSource source;
    public AudioClip sparkSound;
    
    public WaterCounter waterCounter;

    private bool isRising = false; 

    void Start()
    {
        initialPosition = transform.position;
        WaterCounter waterCounter = GetComponent<WaterCounter>();
        sparks.SetActive(false);
    }

    void Update()
    {
        if (IsPipeInTrigger() && !isRising)
        {
            StartCoroutine(Rise());
        }
        else if (!IsPipeInTrigger() && isRising)
        {
            StartCoroutine(Fall());
        }
    }

    bool IsPipeInTrigger()
    {
        if (pipe == null || trigger == null)
        {
            Debug.LogWarning("Pipe or trigger not assigned in the inspector.");
            return false;
        }

        Collider pipeCollider = pipe.GetComponent<Collider>();
        Collider triggerCollider = trigger.GetComponent<Collider>();

        if (pipeCollider == null || triggerCollider == null)
        {
            Debug.LogWarning("Collider component not found on pipe or trigger.");
            return false;
        }

        return pipeCollider.bounds.Intersects(triggerCollider.bounds);
    }

    IEnumerator Rise()
    {
        isRising = true;

        while (transform.position.y < initialPosition.y + riseDistance)
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Fall()
    {
        isRising = false;

        while (transform.position.y > initialPosition.y)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterChecker"))
        {
            waterCounter.waterCount += 1;
            sparks.SetActive(true);
            source.PlayOneShot(sparkSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterChecker"))
        {
            waterCounter.waterCount -= 1;
            sparks.SetActive(false);
        }
    }
}