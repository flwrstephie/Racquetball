using UnityEngine;
using System.Collections;
using TMPro;  // If you're using TextMeshPro for tutorial text.

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public Rigidbody ballPrefab; 
    public Transform ballSpawnPoint; 
    public Transform racquet; 
    public float swingSpeed = 500f; 

    [Range(0, 90)]
    public float angle = 45f; 
    public float power = 10f; 
    public float regularHitForce = 15f; 
    public float underhandHitForce = 30f; 

    private bool isSwinging = false;
    private bool isRegularSwing = false; 
    private GameObject currentBall; 

    // Reference to the tutorial text (assign this in the Inspector)
    public TextMeshProUGUI tutorialText;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float verticalInput = Input.GetAxis("Vertical");     

        // Player movement logic
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Shoot ball if player presses space and there's no current ball
        if (Input.GetKeyDown(KeyCode.Space) && currentBall == null)
        {
            ShootBall();
        }

        // Regular swing
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isRegularSwing = true;
            StartCoroutine(RegularSwing());
        }

        // Underhand swing
        if (Input.GetMouseButtonDown(1) && !isSwinging)
        {
            isRegularSwing = false;
            StartCoroutine(UnderhandSwing());
        }
    }

    void ShootBall()
    {
        if (ballPrefab != null && ballSpawnPoint != null && currentBall == null)
        {
            // Instantiate the ball and shoot it
            Rigidbody ballInstance = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            currentBall = ballInstance.gameObject; 

            // Launch the ball
            LaunchBall(ballInstance);

            // Hide the tutorial text after the first shot
            if (tutorialText != null)
            {
                tutorialText.gameObject.SetActive(false);
            }
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

        Quaternion startRotation = racquet.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -30); 

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

        Quaternion startRotation = racquet.localRotation;
        Quaternion targetRotation = Quaternion.Euler(45, 0, -45); 

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
        if (other.CompareTag("Ball") && isSwinging)
        {
            Rigidbody ballRigidbody = other.GetComponent<Rigidbody>();

            if (ballRigidbody != null)
            {
                float appliedForce = isRegularSwing ? regularHitForce : underhandHitForce;
                Vector3 hitDirection = racquet.transform.forward.normalized;
                ballRigidbody.velocity = hitDirection * appliedForce;

                Debug.Log($"Ball hit with {(isRegularSwing ? "Regular" : "Underhand")} swing! Velocity: {ballRigidbody.velocity}");
            }
        }
    }
    
    public void LoseLife()
    {
        ScoreManager.Instance.LoseLife();
    }
}
