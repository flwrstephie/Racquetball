using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float fixedSpeed = 20f;        // Fixed speed for consistent movement
    public float verticalAdjustment = 2f; // Slight vertical adjustment after wall bounce

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // Ensure the ball uses gravity
        _rigidbody.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Racquet"))
        {
            // Reflect the ball's velocity, ensuring it stays consistent
            Vector3 velocity = _rigidbody.velocity;

            // Neutralize vertical movement and prioritize forward direction
            velocity = new Vector3(velocity.x, 0f, Mathf.Abs(velocity.z));

            // Normalize and apply fixed speed
            _rigidbody.velocity = velocity.normalized * fixedSpeed;

            Debug.Log($"Hit Racquet! Adjusted velocity: {_rigidbody.velocity}");
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Reflect the velocity for wall collision
            Vector3 reflection = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);

            // Add a slight vertical component to prevent sliding
            reflection.y += verticalAdjustment;

            // Normalize and apply fixed speed
            _rigidbody.velocity = reflection.normalized * fixedSpeed;

            Debug.Log($"Hit Wall! Adjusted velocity: {_rigidbody.velocity}");
        }
    }
}
