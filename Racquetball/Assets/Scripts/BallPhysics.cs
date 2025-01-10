using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float bounceMultiplier = 1.5f; // Control bounce strength
    public float minVelocity = 5f;       // Minimum velocity for stability
    public float maxVelocity = 20f;      // Clamp max velocity to prevent extreme bounces

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // Ensure the ball uses gravity
        _rigidbody.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reflect the velocity along the collision normal
        Vector3 reflectedVelocity = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);

        // Stabilize direction by normalizing the velocity vector
        if (reflectedVelocity.magnitude < minVelocity)
        {
            reflectedVelocity = reflectedVelocity.normalized * minVelocity;
        }

        // Amplify the reflected velocity and clamp to max velocity
        Vector3 finalVelocity = reflectedVelocity * bounceMultiplier;
        finalVelocity = Vector3.ClampMagnitude(finalVelocity, maxVelocity);

        // Apply the stabilized velocity
        _rigidbody.velocity = finalVelocity;

        // Optional: Log for debugging
        Debug.Log($"Ball bounced! New velocity: {_rigidbody.velocity}, Collision normal: {collision.contacts[0].normal}");
    }
}
