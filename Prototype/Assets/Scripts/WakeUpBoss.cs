using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WakeUpBoss : MonoBehaviour
{
    //public GameObject liftObject;         // объект, который будет подниматься и опускаться
    public GameObject rotationObject;     // объект, у которого меняется угол поворота
    //public float liftHeight = 5f;         // высота поднятия
    public float liftDuration = 3f;       // длительность поднятия
    public float delayBeforeLower = 2f;  // задержка перед опусканием
    public GameObject bossModel;
    public float bossLiftHeight;

    private HeroCharacterController heroController;
    private FirstBossController bossController;
    private BossAnimator bossAnimator;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isTriggered = false;

    public GameObject Music1;
    public GameObject Music2;

    public AudioClip bossAwake;
    public AudioSource ausioSource;

    void Start()
    {
        initialRotation = rotationObject.transform.rotation;
        
        bossAnimator = bossModel.GetComponent<BossAnimator>();
        bossController = bossModel.GetComponent<FirstBossController>();
        bossController.ActivateBossObjects(false);
        bossController.enabled = false;
        bossModel.SetActive(false);
        
        Music1.SetActive(true);
        Music2.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            ausioSource.PlayOneShot(bossAwake);
            isTriggered = true;
            Music1.SetActive(false);
            Music2.SetActive(true);
            heroController = other.GetComponent<HeroCharacterController>();
            bossModel.SetActive(true);
            Vector3 bossPosition = bossModel.transform.position;
            Vector3 finalBossPosition = bossPosition;
            bossPosition.y -= bossLiftHeight;
            bossModel.transform.position = bossPosition;
            bossAnimator.WakeUp();


            if (heroController != null)
            {
                heroController.SetCharacterState("Idle");
                heroController.enabled = false;
                StartCoroutine(RaiseAndLower());
            }
        }
    }

    IEnumerator RaiseAndLower()
    {
        float elapsedTime = 0f;

        while (elapsedTime < liftDuration)
        {
            float liftHeightPerSecond = bossLiftHeight / liftDuration;

            // Получаем значение тряски по оси X, изменяющееся в течение времени
            float shakeAmount = Mathf.Sin(elapsedTime * 50) * 2;

            // Прибавляем тряску к текущей позиции по оси X
            float newXPosition = initialPosition.x + shakeAmount;

            rotationObject.transform.Rotate(Vector3.left * (10f / liftDuration) * Time.deltaTime);
            bossModel.transform.Translate(Vector3.up * liftHeightPerSecond * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(delayBeforeLower);
        
        bossAnimator.Idle();
        
        elapsedTime = 0f;
        while (elapsedTime < (liftDuration - 2.3f))
        {
            rotationObject.transform.rotation = Quaternion.Slerp(rotationObject.transform.rotation, initialRotation, Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Восстанавливаем контроллер персонажа
        if (heroController != null)
        {
            heroController.enabled = true;
        }

        bossController.enabled = true;
        bossController.Start();
        gameObject.SetActive(false);
    }
}
