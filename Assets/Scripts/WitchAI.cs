using UnityEngine;

public class WitchAI : MonoBehaviour
{
    public Transform fairy;
    public float chaseSpeed = 4f;
    public float trapCooldown = 5f;
    public GameObject trapPrefab;
    public PlanetBounds planetBounds;

    private float trapTimer;

    void Update()
    {
        if (fairy == null || planetBounds == null) return;

        // 1. Центр шара
        Vector3 planetCenter = planetBounds.GetCenter();

        // 2. Направление к Фее
        Vector3 directionToFairy = (fairy.position - transform.position).normalized;

        // 3. Пытаемся двигаться
        Vector3 newPosition = transform.position + directionToFairy * chaseSpeed * Time.deltaTime;

        // 4. Проверка препятствий (Raycast)
        float checkDistance = 1.2f;
        if (directionToFairy != Vector3.zero)
        {
            Ray ray = new Ray(transform.position, directionToFairy);
            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                if (hit.collider.CompareTag("Tree"))
                {
                    // Если впереди дерево, стоим на месте
                    newPosition = transform.position;
                }
            }
        }

        // 5. Прилипаем к шару
        Vector3 directionFromCenter = (newPosition - planetCenter).normalized;
        newPosition = planetCenter + directionFromCenter * planetBounds.walkableRadius;

        // 6. Применяем позицию
        transform.position = newPosition;

        // 7. Поворот
        transform.up = directionFromCenter;

        // 8. Таймер ловушек
        trapTimer += Time.deltaTime;
        if (trapTimer >= trapCooldown)
        {
            trapTimer = 0f;
            if (trapPrefab != null)
            {
                Vector3 trapPos = planetCenter + directionFromCenter * (planetBounds.walkableRadius - 0.2f);
                Instantiate(trapPrefab, trapPos, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ведьма поймала фею!");
        }
    }
}