using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int groundHitCount = 0; // Tracks the number of ground hits

    public float fixedSpeed = 20f; // Fixed speed for consistent movement

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true; // Gravity is applied but vertical velocity will be managed manually
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Racquet"))
        {
            HandleRacquetCollision(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            HandleWallCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            HandleGroundCollision();
        }
    }

    private void HandleRacquetCollision(GameObject racquet)
    {
        // Get the racquet's forward direction
        Vector3 forwardDirection = racquet.transform.forward;

        // Ensure the ball moves in the forward direction with no vertical motion
        Vector3 newVelocity = forwardDirection.normalized * fixedSpeed;

        // Reset the vertical component of velocity to prevent downward motion
        newVelocity.y = 0f;

        // Apply the velocity to the Rigidbody
        _rigidbody.velocity = newVelocity;

        Debug.Log($"Hit Racquet! Adjusted velocity: {_rigidbody.velocity}");
    }

    private void HandleWallCollision(Collision collision)
    {
        // Reflect the ball's velocity based on the collision normal
        Vector3 reflection = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);

        // Neutralize vertical velocity for consistent horizontal motion
        reflection.y = 0f;

        // Apply the normalized speed
        _rigidbody.velocity = reflection.normalized * fixedSpeed;

        Debug.Log($"Hit Wall! Adjusted velocity: {_rigidbody.velocity}");
    }

    private void HandleGroundCollision()
    {
        // Increment the ground hit count
        groundHitCount++;

        Debug.Log($"Hit Ground! Count: {groundHitCount}");

        // Destroy the ball after it hits the ground twice
        if (groundHitCount >= 2)
        {
            Destroy(gameObject);
            Debug.Log("Ball destroyed after hitting the ground twice.");
        }
    }
}
