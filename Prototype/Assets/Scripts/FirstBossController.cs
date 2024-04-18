using System.Collections;
using UnityEngine;

public class FirstBossController : MonoBehaviour
{
    [SerializeField] private GameObject[] bossObjects;
    [SerializeField] private GameObject[] bossModel;
    [SerializeField] private GameObject[] earthquakeObjects;
    [SerializeField] private GameObject[] earthquakeWood;
    [SerializeField] private float activationDuration = 2f;
    [SerializeField] private float deactivationDuration = 2f;
    [SerializeField] private GameObject[] defeatObjects;
    [SerializeField] private GameObject lever;
    [SerializeField] public Collider defeatCollider;
    
    public AudioSource bossAudioSource;
    public AudioClip[] switchSound;
    public AudioClip[] activationSound;
    
    public Transform playerTransform;
    private BossAnimator bossAnimator;
    public HeroCharacterController heroController;
    public GameObject cameraObject; 
    public float moveDistance = 10f;  

    private bool isAlive = true;
    private bool isTriggered = false;

    // Start is called before the first frame update
    public void Start()
    {
        StartCoroutine(BossObjectActivationRoutine());
        foreach (GameObject defeatObject in defeatObjects)
        {
            defeatObject.SetActive(false);
        }
        
        foreach (GameObject earthquakeObject in earthquakeObjects)
        {
            earthquakeObject.SetActive(false);
        }

        bossAnimator = GetComponent<BossAnimator>();
    }

    void Update()
    {
        if (!isTriggered)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && FindObjectOfType<HeroCharacterController>().controlMouse == true && FindObjectOfType<HeroCharacterController>().canGrab && IsPlayerInsideTrigger())
            {
                DefeatBoss();
                isTriggered = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) && FindObjectOfType<HeroCharacterController>().controlMouse == false && FindObjectOfType<HeroCharacterController>().canGrab && IsPlayerInsideTrigger())
            {
                DefeatBoss();
                isTriggered = true;
            } 
        }

    }

    // Coroutine для активации и деактивации объектов босса
    IEnumerator BossObjectActivationRoutine()
    {
        while (isAlive)
        {
            // Активируем объекты босса
            ActivateBossObjects(true);
            bossAudioSource.PlayOneShot(switchSound[Random.Range(0, switchSound.Length)]);

            // Ждем заданное время
            yield return new WaitForSeconds(activationDuration);

            // Деактивируем объекты босса
            ActivateBossObjects(false);
            bossAudioSource.PlayOneShot(activationSound[Random.Range(0, activationSound.Length)]);

            // Ждем заданное время перед следующим циклом
            yield return new WaitForSeconds(deactivationDuration);
        }
    }

    // Метод для активации или деактивации объектов босса
    public void ActivateBossObjects(bool activate)
    {
        foreach (GameObject bossObject in bossObjects)
        {
            bossObject.SetActive(activate);
        }
    }

    // Метод, вызываемый при смерти босса
    public void SetBossDead()
    {
        bossAnimator.Death();
        StartCoroutine(DisableBossModelAfterDelay(5f));
    }

    private IEnumerator DisableBossModelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Добавьте ваш код здесь, который должен выполниться после задержки
        foreach (GameObject model in bossModel)
        {
            model.SetActive(false);
        }

        Earthquake();
    }
    
    // Функция, вызываемая для победы над боссом
    public void DefeatBoss()
    {
        foreach (GameObject bossObject in bossObjects)
        {
            bossObject.SetActive(false);
        }
        isAlive = false;
        // Поворачиваем определенный объект на -90 градусов по оси X за одну секунду
        StartCoroutine(RotateObjectOverTime(lever, new Vector3(-90f, 0f, 0f), 1f));

        StartCoroutine(ActivateObjectsWithDelays());


    }

    // Coroutine для вращения объекта за определенное время
    IEnumerator RotateObjectOverTime(GameObject obj, Vector3 targetRotation, float duration)
    {
        float time = 0f;
        Quaternion initialRotation = obj.transform.rotation;
        Quaternion target = Quaternion.Euler(targetRotation);

        while (time < 1f)
        {
            obj.transform.rotation = Quaternion.Slerp(initialRotation, target, time);
            time += Time.deltaTime / duration;
            yield return null;
        }

        obj.transform.rotation = target;
    }
    
    IEnumerator ActivateObjectsWithDelays()
    {
        if (heroController != null)
        {
            heroController.SetCharacterState("Idle");
            heroController.enabled = false;
            
        }
        yield return new WaitForSeconds(1f);
        defeatObjects[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        defeatObjects[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        defeatObjects[2].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(MoveCamera());
        
        defeatObjects[3].SetActive(true);
        yield return new WaitForSeconds(0.3f);
        defeatObjects[4].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        defeatObjects[5].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        defeatObjects[6].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        defeatObjects[7].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        defeatObjects[8].SetActive(true);
        yield return new WaitForSeconds(1f);
        defeatObjects[9].SetActive(true);
        // Вызываем метод SetBossDead
        SetBossDead();
        Debug.LogError("Boss Defeated");
    }

    IEnumerator MoveCamera()
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = cameraObject.transform.position; // Сохраняем начальную позицию

        while (elapsedTime < 3f)
        {
            // Применяем измененную позицию
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x - (moveDistance/3f) * Time.deltaTime, cameraObject.transform.position.y, cameraObject.transform.position.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Возвращаем объект в изначальное положение
        cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, initialPosition.y, initialPosition.z);

        // Ждем перед опусканием
        yield return new WaitForSeconds(2f);
        

        // Опускаем объект
        elapsedTime = 0f;
        while (elapsedTime < 3f)
        {
            cameraObject.transform.Translate(Vector3.right * (moveDistance / 3f) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraObject.transform.position = initialPosition;

        // Восстанавливаем контроллер персонажа
        if (heroController != null)
        {
            heroController.enabled = true;
        }
    }

    bool IsPlayerInsideTrigger()
    {
        if (defeatCollider != null)
        {
            // Используем метод IsTouching для проверки, находится ли игрок внутри триггера
            return defeatCollider.bounds.Intersects(playerTransform.GetComponent<Collider>().bounds);
        }

        return false;
    }

    void Earthquake()
    {
        foreach (GameObject earthquakeObject in earthquakeObjects)
        {
            earthquakeObject.SetActive(true);
        }

        foreach (GameObject wood in earthquakeWood)
        {
            Rigidbody woodRigid = wood.AddComponent<Rigidbody>();
            Collider woodCollider = wood.AddComponent<BoxCollider>();
            woodRigid.useGravity = true;
        }

    }
}