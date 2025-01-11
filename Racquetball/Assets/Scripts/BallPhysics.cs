using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int groundHitCount = 0; // Tracks the number of ground hits
    private GameObject lastWallHit; // Tracks the last wall the ball hit

    public float fixedSpeed = 20f;       // Fixed speed for consistent movement
    public float bounceSpeed = 10f;      // Vertical speed for bouncing off the ground
    public int maxGroundHits = 2;        // Maximum number of ground bounces before destruction
    private PlayerController playerController;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true; // Gravity is applied but vertical velocity will be managed manually

        // Get reference to PlayerController
        playerController = FindObjectOfType<PlayerController>();
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
        // Reset ground hit count and last wall hit
        groundHitCount = 0;
        lastWallHit = null;

        // Award a point for hitting the racquet
        ScoreManager.Instance.AddPoint();

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

    private int consecutiveWallHits = 0; // Tracks consecutive hits between two walls

    private void HandleWallCollision(Collision collision)
    {
        // Reset ground hit count
        groundHitCount = 0;

        // Increment the consecutive wall hit count
        consecutiveWallHits++;

        // Reflect the ball's velocity based on the collision normal
        Vector3 reflection = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);

        // Add a slight adjustment if the ball is hitting walls repeatedly
        if (consecutiveWallHits > 2)
        {
            Debug.Log("Adjusting trajectory to avoid infinite wall bouncing.");
            reflection.x += Random.Range(-0.5f, 0.5f); // Slightly adjust the x direction
            consecutiveWallHits = 0; // Reset the counter after adjustment
        }

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

        // Apply a bounce effect by setting a vertical velocity
        if (groundHitCount < maxGroundHits)
        {
            Vector3 currentVelocity = _rigidbody.velocity;
            currentVelocity.y = bounceSpeed; // Add vertical speed for bounce
            _rigidbody.velocity = currentVelocity;

            Debug.Log($"Ball bounced! New velocity: {_rigidbody.velocity}");
        }
        else
        {
            // Destroy the ball after it hits the ground maxGroundHits times
            Destroy(gameObject);
            Debug.Log("Ball destroyed after maximum ground hits.");

            // Call the LoseLife method from PlayerController when the ball is destroyed
            if (playerController != null)
            {
                playerController.LoseLife(); // Decreases the player's lives
            }
        }
    }
}
