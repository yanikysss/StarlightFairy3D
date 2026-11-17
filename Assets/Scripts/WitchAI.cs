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
        if (GameManager.Instance.isGameOver) return; // Блокировка
        if (fairy == null || planetBounds == null) return;

        Vector3 planetCenter = planetBounds.GetCenter();
        Vector3 directionToFairy = (fairy.position - transform.position).normalized;
        Vector3 newPosition = transform.position + directionToFairy * chaseSpeed * Time.deltaTime;

        Vector3 directionFromCenter = (newPosition - planetCenter).normalized;
        newPosition = planetCenter + directionFromCenter * planetBounds.walkableRadius;

        // Проверка деревьев (Raycast)
        float checkDistance = 1.2f;
        if (directionToFairy != Vector3.zero)
        {
            Ray ray = new Ray(transform.position, directionToFairy);
            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                if (hit.collider.CompareTag("Tree")) newPosition = transform.position;
            }
        }

        transform.position = newPosition;
        transform.up = directionFromCenter;

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
        if (GameManager.Instance.isGameOver) return;
        if (other.CompareTag("Player")) GameManager.Instance.CaughtByWitch();
    }
}