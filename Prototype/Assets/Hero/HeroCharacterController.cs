using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;
//using UnityEngine.Windows;


public class HeroCharacterController : MonoBehaviour
{
	[SerializeField] public bool controlMouse = true;
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] private GameObject[] heroModel; 
    [SerializeField] private GameObject[] deathScreen; 
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] public bool canJump;
    [SerializeField] public bool canGrab;
    [SerializeField] public bool canFire;
    [SerializeField] public ParticleSystem fireParticleSystem;
    [SerializeField] public Light characterLight;
    [SerializeField] public float horizontalInputSmoothing = 0.2f;
    [SerializeField] public bool isGrounded;
    [SerializeField] private GameObject mainMenuPanel; 
    [SerializeField] private GameObject cheatMenuPanel; 
    
    private GameMaster gm;
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    public CharacterController characterController;
    private float velocity;
    public float horizontalInput;
    public bool isGrabbing;
    private bool readyToGrab;
    private bool readyToDestroy;
    private bool readyToSwitch;
    private GameObject grabbedObject;
    private GameObject destroyObject;
    private GameObject switchingObject;
    private int faceLeftRight;
    public float emissionDuration = 0.1f;
    private bool isFiring = false;
    public float targetIntensity = 15f; // Целевая интенсивность
    public float intensityChangeDuration = 2.0f;
    public bool isOnSlippingSurface;
    private float currentHorizontalInput = 0;
    private bool showMainMenu = true;
    private bool showCheatMenu;
    private float currentSpeed;
    private bool cageDestroy = false;
    public float fadeDuration = 1f;
    public UnityEngine.UI.Image fadeImage;

    //for jumping
    [Header("Jumping")]
    private Collider[] groundCollisions;
    public Vector3 boxSize;
    public float maxDistance;
    
    public GameObject[] groundSensors;
    
    // firing
    [Header("Firing")]
    public GameObject projectilePrefab;  // Ссылка на префаб снаряда
    public float projectileSpeed = 10f;
    public GameObject explosionPrefab; 
    
    // losing abilities
    [Header("Losing Abilities")]
    public GameObject yellowPrefab;
    public GameObject yellowSpawn;
    public GameObject bluePrefab;
    public GameObject blueSpawn;
    public GameObject redPrefab;
    public GameObject redSpawn;
    
    // gaining abilities
    private GameObject followingYellowProjectile;
    private GameObject followingBlueProjectile;
    private GameObject followingRedProjectile;

    //respawn
    private Vector3 respawnPosition; // Добавим переменную для хранения позиции последнего чекпойнта
    private bool isRespawning = false;
    
    //for animation
    [Header("For Animation")]
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset walking;
    public AnimationReferenceAsset running;
    public AnimationReferenceAsset jumping;
    public AnimationReferenceAsset firing;
    public AnimationReferenceAsset pushing;
    public AnimationReferenceAsset pulling;
    public AnimationReferenceAsset lever;
    public string currentState;
    public string currentAnimation;
    
    [Header("Sound")]
    private FootstepSoundController soundController;
    public AudioSource heroAudioSource;
    public AudioClip fireSound;
    public AudioClip explosionSound;
    public AudioClip cageSound;
    public AudioClip loseAbilitiesSound;
    public AudioClip gainYellowSound;
    public AudioClip gainBlueSound;
    public AudioClip gainRedSound;
    public AudioClip deathSound;

    void Start()
    {
        soundController = GetComponent<FootstepSoundController>();
        characterController = GetComponent<CharacterController>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        canJump = gm.canJump;
        canGrab = gm.canGrab;
        canFire = gm.canFire;
        controlMouse = gm.controls;
        currentState = "Idle";
        SetCharacterState(currentState);
        showMainMenu = false;
        showCheatMenu = false;
        ToggleMainMenu(showMainMenu);
        //ToggleCheatMenu(showCheatMenu);
    }

    void Update()
    {
        // Checking current speed
        
        float currentSpeed = horizontalInput;

        // Is Grounded
        isGrounded = CheckIfGrounded();

        //Moving
        if (!isRespawning && characterController.enabled)
        {
            if (controlMouse == true)
            {
                Move(); 
            }
            else
            {
                MoveKeyboard();
            }
        }

        //Grabbing
        if (isGrounded && !isGrabbing && readyToGrab)
        {
            if (controlMouse && Input.GetKeyDown(KeyCode.Mouse1))
            {

                isGrabbing = true;
                if (grabbedObject != null)
                {
                    grabbedObject.transform.parent = transform; // Присоединяем объект к персонажу
                } 
            }
            else if (!controlMouse && Input.GetKeyDown(KeyCode.LeftControl))
            {

                isGrabbing = true;
                if (grabbedObject != null)
                {
                    grabbedObject.transform.parent = transform; // Присоединяем объект к персонажу
                } 
            }

        }
        
        
        //Switching
        
        if (isGrounded && canGrab && !isGrabbing && readyToSwitch)
        {
            if (controlMouse && Input.GetKeyDown(KeyCode.Mouse1))
            {
                Switch();
            }
            else if (!controlMouse && Input.GetKeyDown(KeyCode.LeftControl))
            {
                Switch();
            }
        }
        

        if (controlMouse && Input.GetKeyUp(KeyCode.Mouse1)||!canGrab)
        {
            isGrabbing = false;
            if (grabbedObject != null)
            {
                grabbedObject.transform.parent = null; // Отсоединяем объект от персонажа
            } 
        }
        if (!controlMouse && Input.GetKeyUp(KeyCode.LeftControl)||!canGrab)
        {
            isGrabbing = false;
            if (grabbedObject != null)
            {
                grabbedObject.transform.parent = null; // Отсоединяем объект от персонажа
            } 
        }

        
        //Jumping
        if (isGrounded && canJump && /*!isGrabbing && */ controlMouse && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (isGrounded && canJump && /*!isGrabbing &&*/ !controlMouse && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (isGrounded && canJump && /*!isGrabbing &&*/ !controlMouse && Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        
        // Fire

        if (canFire && !isGrabbing)
        {
            if (controlMouse && Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentState = "Fire";
                SetCharacterState(currentState);
                StartFiring();
            }
            else if (!controlMouse && Input.GetKeyDown(KeyCode.Space))
            {
                currentState = "Fire";
                SetCharacterState(currentState);
                StartFiring();
            }
        }

        //calling the MENU
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMainMenu(showMainMenu);
        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleCheatMenu(showCheatMenu);
        }
        
        
        ApplyGravity();

        // Vertical Velocity
        //characterController.Move(velocity * Time.deltaTime);

        if (followingYellowProjectile != null)
        {
            followingYellowProjectile.transform.position = yellowSpawn.transform.position;  
        }
        
        if (followingBlueProjectile != null)
        {
            followingBlueProjectile.transform.position = blueSpawn.transform.position; 
        }
        
        if (followingRedProjectile != null)
        {
            followingRedProjectile.transform.position = redSpawn.transform.position;
        }

    }
    void FixedUpdate()
    {
        Transform characterTransform = transform;
        characterTransform.position = new Vector3(characterTransform.position.x, characterTransform.position.y, 0f);
    }

    private void Move()
    {
        
        horizontalInput = 0;
        float mouseXPosition = Input.mousePosition.x / Screen.width;
        horizontalInput = (mouseXPosition - 0.5f) * 2f;
        float horizontalInputAbs = Mathf.Abs(horizontalInput);

        if (horizontalInput > 0)
        {
            faceLeftRight = 1;
        }
        else
        {
            faceLeftRight = -1;
        }
        
        // Face forward
        if (!isGrabbing)
        {
            foreach (GameObject model in heroModel)
            {
                Vector3 currentScale = model.transform.localScale;

                model.transform.localScale = new Vector3(Mathf.Abs(currentScale.x) * Mathf.Sign(faceLeftRight), currentScale.y, currentScale.z);
            }
        }
        if (!isOnSlippingSurface && horizontalInputAbs>0.2)
        {
            //Обычное движение
            characterController.Move(new Vector3(horizontalInput * runSpeed, 0, 0) * Time.deltaTime);
        }


        if (isGrounded && !isFiring)
        {
            if (!isGrabbing)
            {
                if (horizontalInputAbs<0.5 )
                {
                    if (horizontalInputAbs < 0.2)
                    {
                        currentState = "Idle";
                    }
                    else
                    {
                        currentState = "Walk";
                    }
                }
                else
                {
                    currentState = "Run";
                } 
            }
            else
            {
                currentState = "Push";
            }

            SetCharacterState(currentState);
        }
    }
    
    private void MoveKeyboard()
    {
        
        horizontalInput = 0;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                horizontalInput = 1.3f;
            }
            else
            {
                horizontalInput = 0.8f;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                horizontalInput = -1.3f;
            }
            else
            {
                horizontalInput = -0.8f;
            }
        }
        float horizontalInputAbs = Mathf.Abs(horizontalInput);
        
        

        if (horizontalInput > 0)
        {
            faceLeftRight = 1;
        }
        else if (horizontalInput < 0)
        {
            faceLeftRight = -1;
        }
        
        // Face forward
        if (!isGrabbing)
        {
            foreach (GameObject model in heroModel)
            {
                Vector3 currentScale = model.transform.localScale;

                model.transform.localScale = new Vector3(Mathf.Abs(currentScale.x) * Mathf.Sign(faceLeftRight), currentScale.y, currentScale.z);
            }
        }
        if (!isOnSlippingSurface && horizontalInputAbs>0.2)
        {
            //Обычное движение
            characterController.Move(new Vector3(horizontalInput * runSpeed, 0, 0) * Time.deltaTime);
        }

        
        if (isGrounded && !isFiring)
        {
            if (!isGrabbing)
            {
                if (horizontalInputAbs<0.5 )
                {
                    if (horizontalInputAbs < 0.2)
                    {
                        currentState = "Idle";
                    }
                    else
                    {
                        currentState = "Walk";
                    }
                }
                else
                {
                    currentState = "Run";
                } 
            }
            else
            {
                currentState = "Push";
            }

            SetCharacterState(currentState);
        }
    }

    private void Jump()
    {
        /*
        if (isGrabbing)
        {
            isGrabbing = false;
                if (grabbedObject != null)
                {
                    grabbedObject.transform.parent = null; // Отсоединяем объект от персонажа
                }
        }
        */

        soundController.Jump();
        velocity += jumpHeight;
        currentState = "Jump";
        SetCharacterState(currentState);
    }
    
    void StartFiring()
    {
        if (!isFiring)
        {

            //StartCoroutine(ActivateEmission());
            StartCoroutine(ChangeLightIntensity(targetIntensity, intensityChangeDuration));


            if (readyToDestroy && destroyObject != null)
            {
                GameObject explosionInstance = Instantiate(explosionPrefab, destroyObject.transform.position, destroyObject.transform.rotation);
                destroyObject.SetActive(false);

                Destroy(destroyObject, 3f);
                Destroy(explosionInstance, 3f);
                destroyObject = null;
                if (cageDestroy)
                {
                    heroAudioSource.volume = 1f;
                    heroAudioSource.PlayOneShot(cageSound);
                }
                else
                {
                    heroAudioSource.volume = 1f;
                    heroAudioSource.PlayOneShot(explosionSound);
                }
            }

            // Создаем новый снаряд
            Vector3 spawnPosition = characterLight.transform.position;
            spawnPosition.y -= 0.3f; // Смещаем по оси Y
            spawnPosition.x += horizontalInput*2f;
            GameObject projectileInstance = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody projectileRigidbody = projectileInstance.GetComponent<Rigidbody>();
            heroAudioSource.volume = 1f;
            heroAudioSource.PlayOneShot(fireSound);

            if (projectileRigidbody == null)
            {
                projectileRigidbody = projectileInstance.AddComponent<Rigidbody>();
            }

            projectileRigidbody.useGravity = false;

            // Направляем снаряд вперед с определенной скоростью
            float direction = (faceLeftRight > 0) ? 1f : -1f;
            projectileRigidbody.velocity = new Vector3(direction * projectileSpeed + horizontalInput*projectileSpeed, 0f, 0f);

            // Запускаем корутину для обработки уничтожения через 2 секунды
            StartCoroutine(DestroyProjectile(projectileInstance, 2f));
        }
    }

    void LoseAbilities()
    {
        if (canJump)
        {
            float direction = (faceLeftRight > 0) ? -1f : 1f;
            Vector3 spawnPosition = yellowSpawn.transform.position;
            spawnPosition.x += (0.3f * direction); // Смещаем по оси Y
            GameObject yellowInstance = Instantiate(yellowPrefab, spawnPosition, Quaternion.identity);
            Rigidbody yellowRigidbody = yellowInstance.GetComponent<Rigidbody>();
            if (yellowRigidbody == null)
            {
                yellowRigidbody = yellowInstance.AddComponent<Rigidbody>();
            }
            
            yellowRigidbody.useGravity = false;


            ConstantForce constantForce = yellowInstance.AddComponent<ConstantForce>();
            //yellowRigidbody.velocity = new Vector3(direction * 15, 0f, 0f);
            constantForce.force = new Vector3(direction * 10f, 25f, 0f);
            StartCoroutine(DestroyProjectile(yellowInstance, 1f));
        }

        if (canGrab)
        {
            float direction = (faceLeftRight > 0) ? -1f : 1f;
            Vector3 spawnPosition = blueSpawn.transform.position;
            spawnPosition.x += (0.3f * direction); // Смещаем по оси Y
            GameObject blueInstance = Instantiate(bluePrefab, spawnPosition, Quaternion.identity);
            Rigidbody blueRigidbody = blueInstance.GetComponent<Rigidbody>();
            if (blueRigidbody == null)
            {
                blueRigidbody = blueInstance.AddComponent<Rigidbody>();
            }
            
            blueRigidbody.useGravity = false;
            
            ConstantForce constantForce = blueInstance.AddComponent<ConstantForce>();
            //blueRigidbody.velocity = new Vector3(direction * 15, 0f, 0f);
            constantForce.force = new Vector3(direction * 10f, 25f, 0f);
            StartCoroutine(DestroyProjectile(blueInstance, 1f));
        }

        if (canFire)
        {
            float direction = (faceLeftRight > 0) ? -1f : 1f;
            Vector3 spawnPosition = redSpawn.transform.position;
            spawnPosition.x += (0.3f * direction); // Смещаем по оси Y
            GameObject redInstance = Instantiate(redPrefab, spawnPosition, Quaternion.identity);
            Rigidbody redRigidbody = redInstance.GetComponent<Rigidbody>();
            if (redRigidbody == null)
            {
                redRigidbody = redInstance.AddComponent<Rigidbody>();
            }
            
            redRigidbody.useGravity = false;

            ConstantForce constantForce = redInstance.AddComponent<ConstantForce>();
            //redRigidbody.velocity = new Vector3(direction * 15, 0f, 0f);
            constantForce.force = new Vector3(direction * 10f, 25f, 0f);
            StartCoroutine(DestroyProjectile(redInstance, 1f));
        }

        //play sound
        if (canJump || canGrab || canFire)
        {
            heroAudioSource.volume = 0.5f;
            heroAudioSource.PlayOneShot(loseAbilitiesSound);
        }

        canGrab = false;
        canJump = false;
        canFire = false;
    }

    void Switch()
    {
        if (switchingObject != null)
        {
            SwitchController switchController = switchingObject.GetComponent<SwitchController>();
            ElectroSwitchController electroSwitchController = switchingObject.GetComponent<ElectroSwitchController>();
            PipeController pipeController = switchingObject.GetComponent<PipeController>();
            CableSwitch cableSwitch = switchingObject.GetComponent<CableSwitch>();
            BossSwitch bossSwitch = switchingObject.GetComponent<BossSwitch>();
            
            currentState = "Lever";
            SetCharacterState(currentState);
                
            if (switchController != null && !switchController.anyMoving)
            {
                if (!switchController.isActive)
                {
                    // Вызываем метод ActivateSwitch() на переключателе
                    switchController.ActivateSwitch();
                }
                else
                {
                    switchController.DeactivateSwitch();
                }
            }

            if (electroSwitchController != null)
            {
                electroSwitchController.SwitchElectro();
            }
            
            if (pipeController != null && !pipeController.anyMoving)
            {
                // Вызываем метод ActivateSwitch() на переключателе
                pipeController.ActivateSwitch();
                
            }
            if (cableSwitch != null)
            {
                cableSwitch.ActivateSwitch();
            }
            
            if (bossSwitch != null)
            {
                Debug.Log("Switch is here!");
                bossSwitch.Activate();
            }
        }
    }

    IEnumerator DestroyProjectile(GameObject projectile, float time)
    {
        yield return new WaitForSeconds(0.2f);

        // Останавливаем снаряд и уничтожаем его через 1 секунду
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.velocity = Vector3.zero;
        }

        yield return new WaitForSeconds(time);

        Destroy(projectile);
    }
    
    private void ApplyGravity()
    {
        if (isGrounded && velocity < 6.0f)
        {
            velocity = -4.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        Vector3 verticalMovement = Vector3.up * velocity;
        characterController.Move(verticalMovement * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death") && !isRespawning)
        {
            // Установим велосити на -3.0
            //velocity = -3.0f;

            // Запустим корутину для респауна через 3 секунды
            StartCoroutine(RespawnAfterDelay(3.0f));
            heroAudioSource.volume = 1f;
            heroAudioSource.PlayOneShot(deathSound);

        }
        
        if (other.CompareTag("Yellow"))
        {
            if (!canJump)
            {
                heroAudioSource.volume = 0.4f;
                heroAudioSource.PlayOneShot(gainYellowSound);

                // Создаем instance проджектайла, который будет следовать за объектом
                float direction = (faceLeftRight > 0) ? -1f : 1f;
                Vector3 spawnPosition = other.transform.position;
                spawnPosition.y += 5f;
                followingYellowProjectile = Instantiate(yellowPrefab, spawnPosition, Quaternion.identity);
                Rigidbody yellowRigidbody = followingYellowProjectile.GetComponent<Rigidbody>();
                if (yellowRigidbody == null)
                {
                    yellowRigidbody = followingYellowProjectile.AddComponent<Rigidbody>();
                }

                yellowRigidbody.useGravity = false;

                // Убеждаемся, что ConstantForce существует
                ConstantForce constantForce = followingYellowProjectile.GetComponent<ConstantForce>();
                if (constantForce == null)
                {
                    constantForce = followingYellowProjectile.AddComponent<ConstantForce>();
                }

                // Начальная установка силы
                constantForce.relativeForce = new Vector3(direction * 5f, 5f, 0f);

                StartCoroutine(DestroyProjectile(followingYellowProjectile, 1f));
                
            }
            canJump = true;
        }
        
        if (other.CompareTag("Green"))
        {
            if (!canGrab)
            {
                heroAudioSource.volume = 0.4f;
                heroAudioSource.PlayOneShot(gainBlueSound);
                
                // Создаем instance проджектайла, который будет следовать за объектом
                float direction = (faceLeftRight > 0) ? -1f : 1f;
                Vector3 spawnPosition = other.transform.position;
                spawnPosition.y += 5f;
                followingBlueProjectile = Instantiate(bluePrefab, spawnPosition, Quaternion.identity);
                Rigidbody blueRigidbody = followingBlueProjectile.GetComponent<Rigidbody>();
                if (blueRigidbody == null)
                {
                    blueRigidbody = followingBlueProjectile.AddComponent<Rigidbody>();
                }

                blueRigidbody.useGravity = false;

                // Убеждаемся, что ConstantForce существует
                ConstantForce constantForce = followingBlueProjectile.GetComponent<ConstantForce>();
                if (constantForce == null)
                {
                    constantForce = followingBlueProjectile.AddComponent<ConstantForce>();
                }

                // Начальная установка силы
                constantForce.relativeForce = new Vector3(direction * 5f, 5f, 0f);

                StartCoroutine(DestroyProjectile(followingBlueProjectile, 1f));
            }
            canGrab = true;
        }
        
        if (other.CompareTag("Red"))
        {
            if (!canFire)
            {
                heroAudioSource.volume = 0.4f;
                heroAudioSource.PlayOneShot(gainRedSound);   
                
                // Создаем instance проджектайла, который будет следовать за объектом
                float direction = (faceLeftRight > 0) ? -1f : 1f;
                Vector3 spawnPosition = other.transform.position;
                spawnPosition.y += 5f;
                followingRedProjectile = Instantiate(redPrefab, spawnPosition, Quaternion.identity);
                Rigidbody redRigidbody = followingRedProjectile.GetComponent<Rigidbody>();
                if (redRigidbody == null)
                {
                    redRigidbody = followingRedProjectile.AddComponent<Rigidbody>();
                }

                redRigidbody.useGravity = false;

                // Убеждаемся, что ConstantForce существует
                ConstantForce constantForce = followingRedProjectile.GetComponent<ConstantForce>();
                if (constantForce == null)
                {
                    constantForce = followingRedProjectile.AddComponent<ConstantForce>();
                }

                // Начальная установка силы
                constantForce.relativeForce = new Vector3(direction * 5f, 5f, 0f);

                StartCoroutine(DestroyProjectile(followingRedProjectile, 1f));
            }
            canFire = true;
        }
        
        if (other.CompareTag("Grabbable"))
        {
            if (canGrab == true)
            {
                readyToGrab = true;
                grabbedObject = other.gameObject;
            }
        }
        
        if (other.CompareTag("Switch"))
        {
            readyToSwitch = true;
            switchingObject = other.gameObject;
        }
        
        if (other.CompareTag("Destroyable"))
        {
            if (canFire)
            {
                readyToDestroy = true;
                destroyObject = other.gameObject;
            }
        }
        
        if (other.CompareTag("WhiteLight"))
        {
            LoseAbilities();
        }
        
        if (other.CompareTag("Slip"))
        {
            isOnSlippingSurface = true;
        }
        
        if (other.CompareTag("cageTrigger"))
        {
            cageDestroy = true;
        }
        
        if (other.CompareTag("Win"))
        {
            
            StartCoroutine(FadeScreen());
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            isGrabbing = false;
            if (grabbedObject != null)
            {
                grabbedObject.transform.parent = null; // Отсоединяем объект от персонажа
            }
            readyToGrab = false;
            grabbedObject = null;
        }
        
        if (other.CompareTag("Destroyable"))
        {
            destroyObject = null;
        }
        
        if (other.CompareTag("Slip"))
        {
            isOnSlippingSurface = false;
        }
        
        if (other.CompareTag("cageTrigger"))
        {
            cageDestroy = false;
        }
        if (other.CompareTag("Switch"))
        {
            readyToSwitch = false;
            switchingObject = null;
        }
    }
    
    IEnumerator ActivateEmission()
    {
        isFiring = true;

        var emission = fireParticleSystem.emission;
        emission.enabled = true; // Включаем Emission
        yield return new WaitForSeconds(emissionDuration); // Ждем указанное время
        emission.enabled = false; // Выключаем Emission
        isFiring = false;
    }
    
    IEnumerator ChangeLightIntensity(float targetIntensity, float duration)
    {
        float startIntensity = 0;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float newIntensity = Mathf.Lerp(targetIntensity, startIntensity, timeElapsed / duration);
            characterLight.intensity = newIntensity;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterLight.intensity = startIntensity; // Убедитесь, что интенсивность точно равна начальной интенсивности
    }

    bool CheckIfGrounded()
    {
        
        if (characterController.isGrounded)
        {
            return true;
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, 0.2f))
            {
                return true;
                
            }
            else
            {
                foreach (GameObject sensor in groundSensors)
                {
                    if (Physics.CheckSphere(sensor.transform.position, 0.2f, groundLayers)) {
                    return true;
                    }
                }
                return false;
            }
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position-transform.up*maxDistance,boxSize);
    }
    
    private IEnumerator RespawnAfterDelay(float delay)
    {
        characterController.enabled = false;
        currentState = "Idle";
        SetCharacterState(currentState);
        // временно выключаем CharacterController, чтобы корректно установить позицию
        isRespawning = true;
        foreach (GameObject dScreen in deathScreen)
        {
            dScreen.SetActive(true);
        }
        float elapsedTime = 0f;
        Color initialColor = fadeImage.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f); // Чисто черный цвет с полной прозрачностью

        while (elapsedTime < 3.0f)
        {
            fadeImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / delay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        fadeImage.color = Color.black;

        // Ждем указанное время
        yield return new WaitForSeconds(delay);

        characterController.enabled = true;

        // Сбрасываем флаг
        isRespawning = false;

        foreach (GameObject dScreen in deathScreen)
        {
            dScreen.SetActive(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        
        if (!string.IsNullOrEmpty(currentAnimation))
        {
            Spine.TrackEntry currentTrack = skeletonAnimation.state.GetCurrent(0);
            
        }
        
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle"))
        {
            SetAnimation(idle, true, 1f);
        }
        if (state.Equals("Walk"))
        {
            SetAnimation(walking, true, (10f*Mathf.Abs(horizontalInput)));
        }
        if (state.Equals("Run"))
        {
            SetAnimation(running, true, (3.5f*Mathf.Abs(horizontalInput)));
        }
        if (state.Equals("Jump"))
        {
            SetAnimation(jumping, false, 2f);
        }
        if (state.Equals("Fire"))
        {
            SetAnimation(firing, false, 4f);
        }
        if (state.Equals("Push"))
        {
            SetAnimation(pushing, true, 1.2f);
        }
        if (state.Equals("Pull"))
        {
            SetAnimation(pulling, true, 1.2f);
        }
        if (state.Equals("Lever"))
        {
            SetAnimation(lever, false, 4f);
        }
    }

    public void ToggleMainMenu(bool show)
    {
        showMainMenu = show;

        if (characterController != null)
        {
            characterController.enabled = !show;
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(show);
        }

        showMainMenu = !show;
    }
    
    public void ToggleCheatMenu(bool show)
    {
        showCheatMenu = show;

        if (cheatMenuPanel != null)
        {
            cheatMenuPanel.SetActive(show);
        }

        showCheatMenu = !show;
    }

    private IEnumerator FadeScreen()
    {
        float elapsedTime = 0f;
        Color initialColor = fadeImage.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f); // Чисто черный цвет с полной прозрачностью

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        fadeImage.color = Color.black;
        gm.lastCheckPointPos = new Vector3(-145f, 14f, 0f);

        SceneManager.LoadScene("WaterLevel");
    }
    
}
