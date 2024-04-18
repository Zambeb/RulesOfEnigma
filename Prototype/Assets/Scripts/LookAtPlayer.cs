using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Collider triggerCollider; // Ссылка на коллайдер триггера

    void Update()
    {
        // Проверяем, находится ли игрок внутри триггера
        if (IsPlayerInsideTrigger())
        {
            // Если игрок внутри триггера, продолжаем смотреть на него
            if (playerTransform != null)
            {
                // Получаем направление от текущей позиции к позиции игрока
                Vector3 directionToPlayer = playerTransform.position - transform.position;

                // Создаем кватернион, который поворачивает объект в направлении игрока
                Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);

                // Применяем этот кватернион к объекту
                transform.rotation = rotationToPlayer;
            }
        }
		else {
		transform.rotation = Quaternion.Euler(154f, -6f, 8f);
		}
    }

    // Проверка, находится ли игрок внутри триггера
    bool IsPlayerInsideTrigger()
    {
        if (triggerCollider != null)
        {
            // Используем метод IsTouching для проверки, находится ли игрок внутри триггера
            return triggerCollider.bounds.Intersects(playerTransform.GetComponent<Collider>().bounds);
        }

        return false;
    }
}
