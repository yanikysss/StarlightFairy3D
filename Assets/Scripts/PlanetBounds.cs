using UnityEngine;

public class PlanetBounds : MonoBehaviour
{
    public float walkableRadius = 5f;

    // Возвращает центр планеты
    public Vector3 GetCenter()
    {
        return transform.position;
    }
}