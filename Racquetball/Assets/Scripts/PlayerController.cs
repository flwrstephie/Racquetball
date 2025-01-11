using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed for left-right movement
    public Rigidbody ballPrefab; // Assign the ball prefab in the Inspector
    public Transform ballSpawnPoint; // Where the ball spawns
    public Transform racquet; // Assign the racquet in the Inspector
    public float swingSpeed = 500f; // Speed of racquet swing

    [Range(0, 90)]
    public float angle = 45f; // Launch angle
    public float power = 10f; // Launch power
    public float regularHitForce = 15f; // Force applied for LMB swing
    public float underhandHitForce = 30f; // Force applied for RMB swing

    private bool isSwinging = false;
    private bool isRegularSwing = false; // Tracks which swing is currently being used

    void Update()
    {
        // Move left and right
        float horizontalInput = Input.GetAxis("Horizontal"); // Left/Right movement (A/D keys or arrow keys)
        float verticalInput = Input.GetAxis("Vertical");     // Forward/Backward movement (W/S keys or arrow keys)

        // Combine horizontal and vertical input into a single movement vector
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;

        // Apply the movement to the player object
        transform.Translate(movement, Space.World);

        // Shoot the ball when Space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBall();
        }

        // Regular swing with the left mouse button (LMB)
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isRegularSwing = true;
            StartCoroutine(RegularSwing());
        }

        // Underhand swing with the right mouse button (RMB)
        if (Input.GetMouseButtonDown(1) && !isSwinging)
        {
            isRegularSwing = false;
            StartCoroutine(UnderhandSwing());
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
        float angleInRadians = angle * Mathf.PI / 180;
        Vector3 localLaunchVelocity = new Vector3(
            0,
            Mathf.Sin(angleInRadians) * power,
            Mathf.Cos(angleInRadians) * power
        );
        Vector3 worldLaunchVelocity = transform.TransformDirection(localLaunchVelocity);
        ball.velocity = worldLaunchVelocity;
    }

    private IEnumerator RegularSwing()
    {
        isSwinging = true;

        // Regular swing animation
        Quaternion startRotation = racquet.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -30); // Adjust as needed

        float elapsedTime = 0f;
        float swingDuration = 0.2f;

        while (elapsedTime < swingDuration)
        {
            racquet.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < swingDuration)
        {
            racquet.localRotation = Quaternion.Lerp(targetRotation, startRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSwinging = false;
    }

    private IEnumerator UnderhandSwing()
    {
        isSwinging = true;

        // Underhand swing animation
        Quaternion startRotation = racquet.localRotation;
        Quaternion targetRotation = Quaternion.Euler(45, 0, -45); // Adjust as needed

        float elapsedTime = 0f;
        float swingDuration = 0.3f;

        while (elapsedTime < swingDuration)
        {
            racquet.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < swingDuration)
        {
            racquet.localRotation = Quaternion.Lerp(targetRotation, startRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSwinging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the racquet hits the ball
        if (other.CompareTag("Ball") && isSwinging)
        {
            Rigidbody ballRigidbody = other.GetComponent<Rigidbody>();

            if (ballRigidbody != null)
            {
                // Determine the force and direction based on the swing type
                float appliedForce = isRegularSwing ? regularHitForce : underhandHitForce;
                Vector3 hitDirection = racquet.transform.forward.normalized; // Ball moves forward from racquet

                // Apply velocity to the ball
                ballRigidbody.velocity = hitDirection * appliedForce;

                Debug.Log($"Ball hit with {(isRegularSwing ? "Regular" : "Underhand")} swing! Velocity: {ballRigidbody.velocity}");
            }
        }
    }
}
