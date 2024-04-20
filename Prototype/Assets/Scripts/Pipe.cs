using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] public Vector2 movementAmount1 = new Vector2(2, 2);
    [SerializeField] public Vector2 movementAmount2 = new Vector2(2, 2);

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 targetPosition1;
    private Vector3 targetPosition2;

    private int position = 1;
    
    public float movementSpeed = 6;
    public bool isMoving = false;
    public bool isMovingBack = false;
    
    public AudioSource platformAudioSource;
    public AudioSource movingAudioSource;
    public AudioClip activationSound;
    public AudioClip deactivationSound;
    public AudioClip movingSound;
    
    void Start()
    {
        initialPosition = transform.position;
        targetPosition1 = initialPosition + new Vector3(movementAmount1.x, movementAmount1.y, 0);
        targetPosition1 = initialPosition + new Vector3(movementAmount2.x, movementAmount2.y, 0);
    }

    public void ActivatePlatform()
    {
        platformAudioSource.PlayOneShot(activationSound);
        if (position < 4)
        {
            if (position == 1 || position == 3)
            {
                targetPosition = initialPosition + new Vector3(movementAmount1.x, movementAmount1.y, 0);
                isMoving = true;
            }
            
            if (position == 2)
            {
                targetPosition = initialPosition + new Vector3(movementAmount2.x, movementAmount2.y, 0);
                isMoving = true;
            }
            position += 1;
        }

        else if (position == 4)
        {
            targetPosition = initialPosition;
            isMoving = true;
            position = 1;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime*movementSpeed);
            if (!movingAudioSource.isPlaying)
            {
                {
                    movingAudioSource.clip = movingSound;
                    movingAudioSource.volume = 1f;
                    movingAudioSource.Play();
                }
            }
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                platformAudioSource.PlayOneShot(deactivationSound);
                isMoving = false;
            }
        }
        
        else
        {
            movingAudioSource.Stop();
        }
    }
}