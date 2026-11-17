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

        Vector3 planetCenter = planetBounds.GetCenter();
        Vector3 upDirection = (transform.position - planetCenter).normalized;

        // Берем направления от камеры, чтобы управление было относительно экрана
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Проецируем на поверхность шара
        Vector3 forwardDirection = Vector3.ProjectOnPlane(camForward, upDirection).normalized;
        Vector3 rightDirection = Vector3.ProjectOnPlane(camRight, upDirection).normalized;

        // Желаемая скорость
        Vector3 targetVelocity = (forwardDirection * moveInput.y + rightDirection * moveInput.x) * moveSpeed;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, 40f * Time.deltaTime);

        // Новая позиция
        Vector3 newPosition = transform.position + currentVelocity * Time.deltaTime;

        // Прилипаем к шару
        Vector3 directionFromCenter = (newPosition - planetCenter).normalized;
        newPosition = planetCenter + directionFromCenter * planetBounds.walkableRadius;

        transform.position = newPosition;

        // Поворот феи
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
            Debug.Log("Звезда собрана!");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Trap"))
        {
            Debug.Log("Попали в ловушку!");
            Destroy(other.gameObject);
        }
    }
}