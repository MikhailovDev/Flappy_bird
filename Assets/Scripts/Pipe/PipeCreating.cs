using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCreating : MonoBehaviour
{
    [SerializeField] private GameObject pipe; 

    private const float _MAX_POSITION = 1.7f;
    private const float _MIN_POSITION = -0.6f;

    private float _timerLimit = 1.8f;
    private float _timer;

    void Start() => PipeCreate();

    void Update()
    {
        if (_timer < _timerLimit)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            PipeCreate();
            _timer = 0;
        }
    }

    private void PipeCreate() => Instantiate(pipe, new Vector3(transform.position.x, Random.Range(_MIN_POSITION, _MAX_POSITION), transform.position.z), transform.rotation);
}
