using Config;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private MapConfig _mapConfig;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float _speed = 30f;

    private Vector3 _direction;

    private void OnEnable() => 
        _inputReader.Moved += SetDirection;

    private void OnDisable() => 
        _inputReader.Moved -= SetDirection;

    private void Update()
    {
        Vector3 direction = new Vector3(_direction.x, _direction.y, _direction.z);
        direction *= _speed * Time.deltaTime;

        transform.Translate(direction, Space.Self);

        LimitByBorders();
    }

    private void SetDirection(Vector3 direction) => 
        _direction = direction;

    private void LimitByBorders()
    {
        float positionX = transform.position.x;
        float positionZ = transform.position.z;

        if (transform.position.x < _mapConfig.MinEdgeSize)
            positionX = _mapConfig.MinEdgeSize;
        else if (transform.position.x > _mapConfig.MaxEdgeSize)
            positionX = _mapConfig.MaxEdgeSize;

        if (transform.position.z < _mapConfig.MinEdgeSize)
            positionZ = _mapConfig.MinEdgeSize;
        else if (transform.position.z > _mapConfig.MaxEdgeSize)
            positionZ = _mapConfig.MaxEdgeSize;

        transform.position = new Vector3(positionX, transform.position.y, positionZ);
    }
}