using System;
using UnityEngine;

public class CollisionListener : MonoBehaviour
{
    private float MAX_DANGEROUS_HEIGHT = 2.73f;

    private bool _isCollidedWithSky;
    private bool _isHitGroundByFirstTime = true;

    public static event Action<string> OnBirdCollided;
    public static event Action<string> OnBirdTriggered;

    private void FixedUpdate()
    {
        if (transform.position.y >= MAX_DANGEROUS_HEIGHT && !_isCollidedWithSky)
        {
            OnBirdCollided?.Invoke("Sky");

            _isCollidedWithSky = true;
        }
    }

    private void OnEnable() => GameLogic.OnGameStarted += GameLogic_OnGameStarted;

    private void OnDisable() => GameLogic.OnGameStarted -= GameLogic_OnGameStarted;

    private void GameLogic_OnGameStarted()
    {
        _isCollidedWithSky = false;
        _isHitGroundByFirstTime = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pipe"))
        {
            OnBirdCollided?.Invoke("Pipe");
        }

        if (collision.collider.CompareTag("Base") && _isHitGroundByFirstTime)
        {
            OnBirdCollided?.Invoke("Base");

            _isHitGroundByFirstTime = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PipeTrigger"))
        {
            OnBirdTriggered?.Invoke("PipeTrigger");
        }
    }
}
