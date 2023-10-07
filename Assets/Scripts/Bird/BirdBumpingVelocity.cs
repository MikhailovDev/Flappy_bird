using UnityEngine;

public class BirdBumpingVelocity : MonoBehaviour
{
    [SerializeField] private BirdController controller;
    [SerializeField] private Rigidbody2D rigidBody;

    public void BirdBumpingInBase() => rigidBody.velocity = new Vector2(0, controller.GroundDistance);

    public void BirdBumpingInPipe() => rigidBody.velocity = new Vector2(0, 2);
}
