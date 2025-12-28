using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _minEdgeMapSize = -50f;
    [SerializeField] private float _maxEdgeMapSize = 50f;
    [SerializeField] private float _speed = 30f;
    
    private void Update()
    {
        Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Vector3 direction = DetermineDirection(viewportPoint);

        direction *= _speed * Time.deltaTime;

        transform.Translate(direction, Space.Self);
        
        LimitByBorders();
    }

    private Vector3 DetermineDirection(Vector3 viewportPoint)
    {
        Vector3 direction = Vector3.zero;

        if (viewportPoint.x <= 0)
            direction.x = -1;
        else if (viewportPoint.x > 1)
            direction.x = 1;
        
        if (viewportPoint.y <= 0)
            direction.z = -1;
        else if (viewportPoint.y >= 1)
            direction.z = 1;
        
        return direction;
    }

    private void LimitByBorders()
    {
        float positionX = transform.position.x;
        float positionZ = transform.position.z;
        
        if (transform.position.x < _minEdgeMapSize)
            positionX = _minEdgeMapSize;
        else if (transform.position.x > _maxEdgeMapSize)
            positionX = _maxEdgeMapSize;
        
        if (transform.position.z < _minEdgeMapSize)
            positionZ = _minEdgeMapSize;
        else if (transform.position.z > _maxEdgeMapSize)
            positionZ = _maxEdgeMapSize;
        
        transform.position = new Vector3(positionX, transform.position.y, positionZ);
    }
}
