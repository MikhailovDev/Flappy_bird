using UnityEngine;

public class BirdController : MonoBehaviour, IMovement
{
    public float GROUND { get; private set; } = 1.32f;

    private const int _DEFAULT_VALUE = 3;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SoundManager audioSource;

    [SerializeField] private float jumpBoost = 3.5f;
    [SerializeField] private float fallingForce = 10;

    public float GroundDistance { get; private set; }

    private bool _isBumped;

    private bool _isMoving = true;

    private void Awake() => GroundDistance = _DEFAULT_VALUE;

    private void OnEnable()
    {
        CollisionListener.OnBirdCollided += CollisionChecking_OnBirdCollided;
        GameLogic.OnGameStarted += LogicScript_OnGameStarted;
        TouchListener.OnScreenTouched += TouchController_OnScreenTouched;
    }

    private void OnDisable()
    {
        CollisionListener.OnBirdCollided -= CollisionChecking_OnBirdCollided;
        GameLogic.OnGameStarted -= LogicScript_OnGameStarted;
        TouchListener.OnScreenTouched -= TouchController_OnScreenTouched;
    }

    void Update() => rigidBody.rotation = rigidBody.velocity.y * fallingForce;

    public void StartMoving() => _isMoving = true;

    public void StopMoving() => _isMoving = false;

    private void LogicScript_OnGameStarted() => _isBumped = false;

    private void CollisionChecking_OnBirdCollided(string nameOfCollidedObject)
    {
        _isBumped = true;

        if (nameOfCollidedObject == "Pipe")
        {
            GroundDistance = SetGroundDistance();
        }
    }

    private void TouchController_OnScreenTouched()
    {
        if (!_isBumped && _isMoving)
        {
            rigidBody.velocity = Vector2.up * jumpBoost;

            audioSource.PlayWing();

            GroundDistance = SetGroundDistance();
        }
    }

    private float SetGroundDistance() => transform.position.y + GROUND;
}
