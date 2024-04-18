using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveController : MonoBehaviour
{
    [SerializeField] public Vector2 movementAmount = new Vector2(2, 2);

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    public float movementSpeed = 3;
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
        targetPosition = initialPosition + new Vector3(movementAmount.x, movementAmount.y, 0);
    }

    public void ActivatePlatform()
    {
        platformAudioSource.PlayOneShot(activationSound);
        targetPosition = initialPosition + new Vector3(movementAmount.x, movementAmount.y, 0);
        isMoving = true;
        isMovingBack = false;
    }
    
    public void DeactivatePlatform()
    {
        platformAudioSource.PlayOneShot(activationSound);
        isMovingBack = true;
        isMoving = false;
    }

    void Update()
    {
        if (isMoving)
        {
            // Передвигаем платформу к целевой позиции
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime*movementSpeed);
            


            // Проверяем, достигла ли платформа целевой позиции
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                platformAudioSource.PlayOneShot(deactivationSound);
                isMoving = false;
            }
        }
        
        
        if (isMovingBack)
        {
            // Передвигаем платформу к целевой позиции
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, Time.deltaTime*movementSpeed);

            // Проверяем, достигла ли платформа целевой позиции
            if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
            {
                platformAudioSource.PlayOneShot(deactivationSound);
                isMovingBack = false;
            }
        }

        if (isMoving || isMovingBack)
        {
            if (!movingAudioSource.isPlaying)
            {
                {
                    movingAudioSource.clip = movingSound;
                    movingAudioSource.volume = 1f;
                    movingAudioSource.Play();
                }
            }
        }
        else
        {
            movingAudioSource.Stop();
        }
    }
}