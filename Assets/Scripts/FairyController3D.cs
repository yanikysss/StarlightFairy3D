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
        if (GameManager.Instance.isGameOver) return; // Блокировка после конца игры
        if (planetBounds == null) return;

        Vector3 planetCenter = planetBounds.GetCenter();
        Vector3 upDirection = (transform.position - planetCenter).normalized;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        Vector3 forwardDirection = Vector3.ProjectOnPlane(camForward, upDirection).normalized;
        Vector3 rightDirection = Vector3.ProjectOnPlane(camRight, upDirection).normalized;

        Vector3 targetVelocity = (forwardDirection * moveInput.y + rightDirection * moveInput.x) * moveSpeed;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, 40f * Time.deltaTime);

        // Проверка деревьев (Raycast)
        float checkDistance = 1.2f;
        Vector3 moveDir = currentVelocity.normalized;
        if (moveDir != Vector3.zero)
        {
            Ray ray = new Ray(transform.position, moveDir);
            if (Physics.Raycast(ray, out RaycastHit hit, checkDistance))
            {
                if (hit.collider.CompareTag("Tree")) currentVelocity = Vector3.zero;
            }
        }

        Vector3 newPosition = transform.position + currentVelocity * Time.deltaTime;
        Vector3 directionFromCenter = (newPosition - planetCenter).normalized;
        newPosition = planetCenter + directionFromCenter * planetBounds.walkableRadius;

        transform.position = newPosition;

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