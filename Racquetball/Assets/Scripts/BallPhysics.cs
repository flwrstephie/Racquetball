using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float fixedSpeed = 20f; // Fixed speed for consistent movement

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true; // Gravity is still applied globally, but we'll manage vertical velocity
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Racquet"))
        {
            HandleRacquetCollision();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
    }

    private void HandleRacquetCollision()
    {
        // Reset vertical velocity and maintain forward speed
        Vector3 velocity = _rigidbody.velocity;

        // Neutralize vertical movement (y = 0) and prioritize horizontal/forward movement
        velocity = new Vector3(velocity.x, 0f, Mathf.Abs(velocity.z));

        // Apply the fixed speed consistently
        _rigidbody.velocity = velocity.normalized * fixedSpeed;

        Debug.Log($"Hit Racquet! Adjusted velocity: {_rigidbody.velocity}");
    }

    private void HandleWallCollision(Collision collision)
    {
        // Reflect velocity based on collision normal
        Vector3 reflection = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);

        // Ensure the reflected velocity maintains horizontal/forward direction
        reflection.y = 0f;

        // Normalize and apply fixed speed
        _rigidbody.velocity = reflection.normalized * fixedSpeed;

        Debug.Log($"Hit Wall! Adjusted velocity: {_rigidbody.velocity}");
    }
}
