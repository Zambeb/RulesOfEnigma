using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] player; // Ссылка на объект персонажа
    public Transform triggerCenter; // Ссылка на центральную точку триггера
    [SerializeField] private GameObject[] cameraFocus; // Ссылка на объект, к которому прикреплена камера

    public float maxDistance = 10f; // Максимальное расстояние от центра триггера
    private float multiplierAtMaxDistance = 1f; // Множитель при максимальном расстоянии (персонаж далеко)
    private float multiplierAtCenter = 2f; // Множитель в центре триггера (персонаж близко)

    void Update()
    {
        // Определение положения персонажа относительно центра триггера
        float distanceToCenter = Vector3.Distance(player[0].transform.position, triggerCenter.position);

        // Рассчитываем множитель в зависимости от близости персонажа к центру триггера
        float multiplier = Mathf.Lerp(multiplierAtMaxDistance, multiplierAtCenter, distanceToCenter / maxDistance);

        // Постепенно изменяем координату z объекта cameraFocus
        float smoothness = 5f; // Задает, насколько плавно будет происходить изменение расстояния
        float targetZ = -multiplier * maxDistance; // targetZ - это новое значение координаты z
        Vector3 targetPosition = new Vector3(cameraFocus[0].transform.position.x, cameraFocus[0].transform.position.y, targetZ);
        cameraFocus[0].transform.position = Vector3.Lerp(cameraFocus[0].transform.position, targetPosition, Time.deltaTime * smoothness);
    }
}


