using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed for left-right movement
    public Rigidbody ballPrefab; // Assign the ball prefab in the Inspector
    public Transform ballSpawnPoint; // Where the ball spawns

    [Range(0, 90)]
    public float angle = 45f; // Launch angle
    public float power = 10f; // Launch power

    void Update()
    {
        // Move left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Shoot the ball when Space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
        }
    }

    void ShootBall()
    {
        if (ballPrefab != null && ballSpawnPoint != null)
        {
            Rigidbody ballInstance = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
            LaunchBall(ballInstance);
        }
    }

    void LaunchBall(Rigidbody ball)
{
    // Convert angle to radians
    float angleInRadians = angle * Mathf.PI / 180;

    // Calculate velocity in local space (relative to the player)
    Vector3 localLaunchVelocity = new Vector3(
        0, // No horizontal movement relative to the player
        Mathf.Sin(angleInRadians) * power, // Vertical component
        Mathf.Cos(angleInRadians) * power  // Forward (Z-axis) component
    );

    // Convert to world space using the player's rotation
    Vector3 worldLaunchVelocity = transform.TransformDirection(localLaunchVelocity);

    // Apply the velocity to the ball
    ball.velocity = worldLaunchVelocity;
}

}
