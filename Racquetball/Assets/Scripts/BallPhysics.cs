using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int groundHitCount = 0; 
    private GameObject lastWallHit; 

    public float fixedSpeed = 20f;       
    public float bounceSpeed = 10f;      
    public int maxGroundHits = 2;        
    private PlayerController playerController;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true; 

        
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
        groundHitCount = 0;
        
        ScoreManager.Instance.AddPoint();
        Vector3 forwardDirection = racquet.transform.forward;
        Vector3 newVelocity = forwardDirection.normalized * fixedSpeed;

        newVelocity.y = 0f;
        _rigidbody.velocity = newVelocity;
    }

    private int consecutiveWallHits = 0; 

    private void HandleWallCollision(Collision collision)
    {  
        groundHitCount = 0;
        consecutiveWallHits++;        
        Vector3 reflection = Vector3.Reflect(_rigidbody.velocity, collision.contacts[0].normal);
        
        if (consecutiveWallHits > 2)
        {
            reflection.x += Random.Range(-0.5f, 0.5f); 
            consecutiveWallHits = 0; 
        }
        
        reflection.y = 0f;
        
        _rigidbody.velocity = reflection.normalized * fixedSpeed;
    }

    private void HandleGroundCollision()
    {
        groundHitCount++;
        
        if (groundHitCount < maxGroundHits)
        {
            Vector3 currentVelocity = _rigidbody.velocity;
            currentVelocity.y = bounceSpeed; 
            _rigidbody.velocity = currentVelocity;
        }
        else
        {
            Destroy(gameObject);

            if (playerController != null)
            {
                playerController.LoseLife(); 
            }
        }
    }
}
