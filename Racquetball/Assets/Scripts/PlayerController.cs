using UnityEngine;
using System.Collections; // Needed for IEnumerator

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

    private bool isSwinging = false;

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

        // Swing the racquet when the left mouse button (LMB) is pressed
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(SwingRacquet());
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

    private IEnumerator SwingRacquet()
    {
        isSwinging = true;

        // Save the initial and target rotation
        Quaternion startRotation = racquet.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 45); // Adjust as needed

        float elapsedTime = 0f;
        float swingDuration = 0.2f; // Adjust for swing speed

        // Rotate the racquet to the target position
        while (elapsedTime < swingDuration)
        {
            racquet.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return the racquet to the starting position
        elapsedTime = 0f;
        while (elapsedTime < swingDuration)
        {
            racquet.localRotation = Quaternion.Lerp(targetRotation, startRotation, elapsedTime / swingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSwinging = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                // Reflect the ball's velocity based on the racquet's swing
                Vector3 reflection = Vector3.Reflect(ballRigidbody.velocity, collision.contacts[0].normal);
                ballRigidbody.velocity = reflection * 1.2f; // Adjust bounce strength
            }
        }
    }
}
