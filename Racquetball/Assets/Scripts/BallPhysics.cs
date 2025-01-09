using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // Ensure the ball uses gravity and bounces
        _rigidbody.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle bounce logic
        // Reverse velocity along the axis of the collision normal
        Vector3 reflectedVelocity = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);

        // Apply the reflected velocity to make the ball bounce
        _rigidbody.velocity = reflectedVelocity;
    }
}
