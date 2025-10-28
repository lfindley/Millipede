using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Dart : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 5f; // destroys itself after 5 seconds

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;  // no falling
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // better accuracy
        rb.freezeRotation = true;
    }

    private void Start()
    {
        // Fire upward in local space
        rb.velocity = transform.up * speed;

        // Destroy after some time so they don't pile up
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy on impact
        Destroy(gameObject);
    }
}
