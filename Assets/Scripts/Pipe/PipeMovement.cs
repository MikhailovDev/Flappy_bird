using UnityEngine;

public class PipeMovement : MonoBehaviour, IMovement
{
    private float _DEAD_ZONE_VALUE = -2.5f;

    [SerializeField] private float speed = 1;

    private bool _isMoving = true;

    void FixedUpdate()
    {
        if (_isMoving)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }

        if (transform.position.x < _DEAD_ZONE_VALUE)
        {
            Destroy(gameObject);
        }
    }

    public void StartMoving() => _isMoving = true;
    public void StopMoving() => _isMoving = false;
}
