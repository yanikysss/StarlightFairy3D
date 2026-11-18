using UnityEngine;
using UnityEngine.InputSystem;

public class FairyController3D : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float rotationSpeed = 10f;
    public PlanetBounds planetBounds;

    [Header("Joystick")]
    public SimpleJoystick joystick; // Сюда перетащим объект JoystickBackground

    private Vector2 moveInput;
    private Vector3 currentVelocity;

    // Этот метод вызывается автоматически через Player Input (для клавиатуры)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver) return;
        if (planetBounds == null) return;

        // 1. Центр шара и направление "вверх"
        Vector3 planetCenter = planetBounds.GetCenter();
        Vector3 upDirection = (transform.position - planetCenter).normalized;

        // 2. Оси относительно камеры
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        Vector3 forwardDirection = Vector3.ProjectOnPlane(camForward, upDirection).normalized;
        Vector3 rightDirection = Vector3.ProjectOnPlane(camRight, upDirection).normalized;

        // 3. ВЫБОР ВВОДА: Клавиатура или Джойстик
        Vector2 finalInput = moveInput;

        // Если джойстик назначен и на нем есть ввод (ручка сдвинута)
        if (joystick != null && joystick.GetInput().magnitude > 0.1f)
        {
            finalInput = joystick.GetInput();
        }

        // 4. Желаемая скорость
        Vector3 targetVelocity = (forwardDirection * finalInput.y + rightDirection * finalInput.x) * moveSpeed;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, 40f * Time.deltaTime);

        // 5. Проверка столкновения с деревьями (Raycast)
        float checkDistance = 1.2f;
        Vector3 moveDir = currentVelocity.normalized;

        if (moveDir != Vector3.zero)
        {
            Ray ray = new Ray(transform.position, moveDir);
            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                if (hit.collider.CompareTag("Tree"))
                {
                    currentVelocity = Vector3.zero;
                }
            }
        }

        // 6. Новая позиция и прилипание к шару
        Vector3 newPosition = transform.position + currentVelocity * Time.deltaTime;
        Vector3 directionFromCenter = (newPosition - planetCenter).normalized;
        newPosition = planetCenter + directionFromCenter * planetBounds.walkableRadius;

        transform.position = newPosition;

        // 7. Поворот феи
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
        if (GameManager.Instance.isGameOver) return;

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