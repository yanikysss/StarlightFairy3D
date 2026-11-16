using UnityEngine;

public class PlanetBounds : MonoBehaviour
{
    // Радиус, по которому можно ходить (шар у нас 10, значит 9.5 — это чуть внутри)
    public float walkableRadius = 9.5f;

    public Vector3 ClampPosition(Vector3 position)
    {
        Vector3 center = transform.position;
        Vector3 offset = position - center;

        if (offset.magnitude > walkableRadius)
        {
            offset = offset.normalized * walkableRadius;
            return center + offset;
        }
        return position;
    }

    public bool IsInside(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= walkableRadius;
    }
}
