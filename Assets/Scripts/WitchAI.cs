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

        // 1. Ведьма смотрит на фею (только по плоскости)
        Vector3 direction = (fairy.position - transform.position).normalized;

        // 2. Двигаемся в сторону феи
        Vector3 newPosition = transform.position + direction * chaseSpeed * Time.deltaTime;

        // 3. Прилипаем к поверхности шара (так же, как фея)
        Vector3 planetCenter = planetBounds.GetCenter();
        Vector3 directionFromCenter = (newPosition - planetCenter).normalized;
        newPosition = planetCenter + directionFromCenter * planetBounds.walkableRadius;

        transform.position = newPosition;

        // 4. Поворачиваем ведьму лицом к центру шара (чтобы она не висела как палка)
        transform.up = directionFromCenter;

        // 5. Таймер для ловушек
        trapTimer += Time.deltaTime;
        if (trapTimer >= trapCooldown)
        {
            trapTimer = 0f;
            PlaceTrap();
        }
    }

    void PlaceTrap()
    {
        if (trapPrefab != null)
        {
            // Опускаем тучу чуть ниже, чтобы она лежала на шаре
            Vector3 trapPos = transform.position;
            Instantiate(trapPrefab, trapPos, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ведьма поймала фею! Игра окончена.");
            // Позже здесь будет вызов GameManager
        }
    }
}