using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Перетащите Фею
    public float distance = 8f; // Расстояние от феи
    public float height = 4f;   // Высота камеры
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Находим центр шара
        PlanetBounds bounds = target.GetComponent<FairyController3D>().planetBounds;
        if (bounds == null) return;
        Vector3 planetCenter = bounds.GetCenter();

        // 2. Направление "вверх" для феи (от центра к фее)
        Vector3 upDirection = (target.position - planetCenter).normalized;

        // 3. Берем направление взгляда феи (куда она бежит)
        Vector3 forwardDirection = target.forward;

        // 4. Вычисляем позицию камеры (сзади и сверху)
        Vector3 desiredPosition = target.position - forwardDirection * distance + upDirection * height;

        // 5. Плавно двигаем камеру к этой точке
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 6. САМОЕ ГЛАВНОЕ: Камера смотрит на фею, 
        // но её "верх" всегда направлен от центра шара (чтобы не переворачивалась)
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, upDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}