using UnityEngine;

public class PositionCalculator : MonoBehaviour
{
    private const float CircleDegrees = 360f;
        
    [SerializeField] private float _spawnRadius = 2f;

    private Vector3[] positions;
        
    public Vector3[] CalculateSpawnPositions(int unitsCount)
    {
        positions = new Vector3[unitsCount];
            
        float angleStep = CircleDegrees / unitsCount;

        for (int i = 0; i < unitsCount; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;

            float x = transform.position.x + Mathf.Cos(angle) * _spawnRadius;
            float z = transform.position.z + Mathf.Sin(angle) * _spawnRadius;

            positions[i] = new Vector3(x, 0, z);
        }

        return positions;
    }
}