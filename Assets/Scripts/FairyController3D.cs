using UnityEngine;
using UnityEngine.InputSystem;

public class FairyController3D : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float rotationSpeed = 10f;
    public PlanetBounds planetBounds;

    private Vector2 moveInput;
    private Vector3 currentVelocity;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        if (planetBounds == null) return;

        // 1. Берем центр шара
        Vector3 planetCenter = planetBounds.GetCenter();
        Vector3 upDirection = (transform.position - planetCenter).normalized;

        // 2. Берем направления от камеры
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // 3. Проецируем на поверхность шара
        Vector3 forwardDirection = Vector3.ProjectOnPlane(camForward, upDirection).normalized;
        Vector3 rightDirection = Vector3.ProjectOnPlane(camRight, upDirection).normalized;

        // 4. Вычисляем желаемую скорость
        Vector3 targetVelocity = (forwardDirection * moveInput.y + rightDirection * moveInput.x) * moveSpeed;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, 40f * Time.deltaTime);

        // 5. Проверка препятствий (Raycast)
        float checkDistance = 1.2f; // Дистанция проверки перед феей
        Vector3 moveDir = currentVelocity.normalized;

        if (moveDir != Vector3.zero)
        {
            Ray ray = new Ray(transform.position, moveDir);
            // Если луч врезается в дерево
            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                if (hit.collider.CompareTag("Tree"))
                {
                    // Отменяем движение
                    currentVelocity = Vector3.zero;
                }
            }
        }

        // 6. Новая позиция
        Vector3 newPosition = transform.position + currentVelocity * Time.deltaTime;

        // 7. Прилипаем к шару
        Vector3 directionFromCenter = (newPosition - planetCenter).normalized;
        newPosition = planetCenter + directionFromCenter * planetBounds.walkableRadius;

        transform.position = newPosition;

        // 8. Поворот феи
        if (currentVelocity.sqrMagnitude > 0.1f)
        {
            Vector3 lookDir = currentVelocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(lookDir, upDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Quaternion uprightRotation = Quaternion.FromToRotation(transform.up, upDirection) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            GameManager.Instance.AddStar();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Trap"))
        {
            GameManager.Instance.HitTrap();
            Destroy(other.gameObject);
        }
    }
}