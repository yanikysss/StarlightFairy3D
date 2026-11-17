using UnityEngine;

public class PlanetBounds : MonoBehaviour
{
    public float walkableRadius = 15.5f;

    public Vector3 GetCenter()
    {
        return transform.position;
    }
}