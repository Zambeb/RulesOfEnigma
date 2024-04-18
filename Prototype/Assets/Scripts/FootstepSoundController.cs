using System.Collections;
using UnityEngine;
public class FootstepSoundController : MonoBehaviour
{
    public AudioClip[] walkSounds;
    public AudioClip[] runSounds;
    public AudioClip[] jumpSounds;
    
    public AudioClip[] walkSoundsWood;
    public AudioClip[] runSoundsWood;
    public AudioClip[] jumpSoundsWood;

    public AudioClip[] walkSoundsStone;
    public AudioClip[] runSoundsStone;
    public AudioClip[] jumpSoundsStone;
    
    public AudioClip[] walkSoundsWater;
    public AudioClip[] runSoundsWater;
    public AudioClip[] jumpSoundsWater;
    
    public AudioSource footstepAudioSource;
    
    // dragging
    public AudioSource dragAudioSource;
    public AudioClip draggingSound;
    public AudioClip draggingSoundWood;
    
    public float volume = 0.3f;
    private float walkVolume;
    private float runVolume;

    private HeroCharacterController heroController;
    public Transform playerTransform;
    [SerializeField] public Collider[] woodColliders;
    [SerializeField] public Collider[] stoneColliders;
    [SerializeField] public Collider[] waterColliders;

    private float horizontalInput;
    

    void Start()
    {
        heroController = GetComponent<HeroCharacterController>();
        footstepAudioSource = gameObject.AddComponent<AudioSource>();
        footstepAudioSource.spatialBlend = 0f; // 2D sound
        footstepAudioSource.volume = volume;
        walkVolume = volume + 0.2f;
        runVolume = volume;
        dragAudioSource = gameObject.AddComponent<AudioSource>();
        dragAudioSource.spatialBlend = 0f; // 2D sound
        
    }

    void Update()
    {
        horizontalInput = Mathf.Abs(heroController.horizontalInput);
        if (heroController != null)
        {
            if (horizontalInput > 0.2 && horizontalInput < 0.5)
            {
                if (IsPlayerOnWood() == true)
                {
                    PlayWalkSoundWood(horizontalInput); 
                }
                else if (IsPlayerOnWater() == true)
                {
                    PlayWalkSoundWater(horizontalInput); 
                }
                else if (IsPlayerOnStone() == true)
                {
                    PlayWalkSoundStone(horizontalInput); 
                }
                else
                {
                    PlayWalkSound(horizontalInput); 
                }
            }
            if (horizontalInput > 0.5)
            {
                if (IsPlayerOnWood() == true)
                {
                    PlayRunSoundWood(horizontalInput); 
                }
                else if (IsPlayerOnWater() == true)
                {
                    PlayRunSoundWater(horizontalInput); 
                }
                else if (IsPlayerOnStone() == true)
                {
                    PlayRunSoundStone(horizontalInput); 
                }
                else
                {
                    PlayRunSound(horizontalInput); 
                }
            }
        }
        
        // Dragging
        if (heroController.isGrabbing && horizontalInput > 0.1)
        {
            if (!dragAudioSource.isPlaying)
            {
                if (IsPlayerOnWood())
                {
                    dragAudioSource.clip = draggingSoundWood;
                    dragAudioSource.volume = 0.5f;
                    dragAudioSource.Play();
                }
                else
                {
                    dragAudioSource.clip = draggingSound;
                    dragAudioSource.volume = 1;
                    dragAudioSource.Play();
                }
            }
        }
        else
        {
            dragAudioSource.Stop();
        }
    }
    
    // Getting random sound
    AudioClip GetRandomSound(AudioClip[] sounds)
    {
        if (sounds.Length > 0)
        {
            return sounds[Random.Range(0, sounds.Length)];
        }
        return null;
    }

    // Walk sounds
    
    void PlayWalkSound(float horizontalInput)
    {
        if (heroController.isGrounded && heroController.enabled)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip walkSound = GetRandomSound(walkSounds);
                footstepAudioSource.volume = walkVolume;
                footstepAudioSource.PlayOneShot(walkSound);
            }
        }
    }

    void PlayRunSound(float horizontalInput)
    {
        if (heroController.isGrounded && heroController.enabled)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip runSound = GetRandomSound(runSounds);
                footstepAudioSource.volume = runVolume;
                footstepAudioSource.PlayOneShot(runSound);
            }
        }
    }

    void PlayWalkSoundWood(float horizontalInput)
    {
        if (heroController.isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip walkSoundWood = GetRandomSound(walkSoundsWood);
                footstepAudioSource.volume = 1;
                footstepAudioSource.PlayOneShot(walkSoundWood);
            }
        }
    }
    
    void PlayRunSoundWood(float horizontalInput)
    {
        if (heroController.isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip runSoundWood = GetRandomSound(runSoundsWood);
                footstepAudioSource.volume = 1;
                footstepAudioSource.PlayOneShot(runSoundWood);
            }
        }
    }
    
    void PlayWalkSoundStone(float horizontalInput)
    {
        if (heroController.isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip walkSoundStone = GetRandomSound(walkSoundsStone);
                footstepAudioSource.volume = 0.5f;
                footstepAudioSource.pitch = 1.0f + horizontalInput/2; 
                footstepAudioSource.PlayOneShot(walkSoundStone);
            }
        }
    }
    
    void PlayRunSoundStone(float horizontalInput)
    {
        if (heroController.isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip runSoundStone = GetRandomSound(runSoundsStone);
                footstepAudioSource.volume = 0.5f;
                footstepAudioSource.pitch = 1.0f + horizontalInput/2; 
                footstepAudioSource.PlayOneShot(runSoundStone);
            }
        }
    }
    
    void PlayWalkSoundWater(float horizontalInput)
    {
        if (heroController.isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip walkSoundWater = GetRandomSound(walkSoundsWater);
                footstepAudioSource.volume = 1;
                footstepAudioSource.pitch = 1.0f + horizontalInput/2; 
                footstepAudioSource.PlayOneShot(walkSoundWater);
            }
        }
    }
    
    void PlayRunSoundWater(float horizontalInput)
    {
        if (heroController.isGrounded)
        {
            if (!footstepAudioSource.isPlaying)
            {
                AudioClip runSoundWater = GetRandomSound(runSoundsWater);
                footstepAudioSource.volume = 1f;
                footstepAudioSource.pitch = 1.0f + horizontalInput/2; 
                footstepAudioSource.PlayOneShot(runSoundWater);
            }
        }
    }

    public void Jump()
    {
        if (IsPlayerOnWood() == true)
        {
            PlayJumpSoundWood(horizontalInput); 
        }
        else if (IsPlayerOnStone() == true)
        {
            PlayJumpSoundStone(horizontalInput); 
        }
        else if (IsPlayerOnWater() == true)
        {
            PlayJumpSoundWater(horizontalInput); 
        }
        else
        {
            PlayJumpSound(horizontalInput); 
        }
    }
    void PlayJumpSound(float horizontalInput)
    {
        if (!footstepAudioSource.isPlaying)
            {
                AudioClip jumpSound = GetRandomSound(jumpSounds);
                footstepAudioSource.volume = 1;
                footstepAudioSource.PlayOneShot(jumpSound);
            }
    }

    void PlayJumpSoundWood(float horizontalInput)
    {
        if (!footstepAudioSource.isPlaying)
            {
                AudioClip jumpSoundWood = GetRandomSound(jumpSoundsWood);
                footstepAudioSource.volume = 1;
                footstepAudioSource.PlayOneShot(jumpSoundWood);
            }
    }
    
    void PlayJumpSoundStone(float horizontalInput)
    {
        if (!footstepAudioSource.isPlaying)
        {
            AudioClip jumpSoundStone = GetRandomSound(jumpSoundsStone);
            footstepAudioSource.volume = 1;
            footstepAudioSource.PlayOneShot(jumpSoundStone);
        }
    }
    
    void PlayJumpSoundWater(float horizontalInput)
    {
        if (!footstepAudioSource.isPlaying)
        {
            AudioClip jumpSoundWater = GetRandomSound(jumpSoundsWater);
            footstepAudioSource.volume = 1;
            footstepAudioSource.PlayOneShot(jumpSoundWater);
        }
    }

    
    // CHECKING COLLIDERS

    bool IsPlayerOnWood()
    {
        if (woodColliders != null && playerTransform != null)
        {
            foreach (Collider woodCollider in woodColliders)
            {
                if (woodCollider.bounds.Intersects(playerTransform.GetComponent<Collider>().bounds))
                {
                    return true; 
                }
            }
        }

        return false;
    }
    
    bool IsPlayerOnStone()
    {
        if (stoneColliders != null && playerTransform != null)
        {
            foreach (Collider stoneCollider in stoneColliders)
            {
                if (stoneCollider.bounds.Intersects(playerTransform.GetComponent<Collider>().bounds))
                {
                    return true; 
                }
            }
        }

        return false;
    }
    
    bool IsPlayerOnWater()
    {
        if (waterColliders != null && playerTransform != null)
        {
            foreach (Collider waterCollider in waterColliders)
            {
                if (waterCollider.bounds.Intersects(playerTransform.GetComponent<Collider>().bounds))
                {
                    return true; 
                }
            }
        }

        return false;
    }

}