using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Downward speed in units per second")]
    public float fallSpeed = 3.2f;
    [Tooltip("Enable gentle left-right sway while falling")]
    public bool sway = true;
    [Tooltip("Horizontal speed (units/sec) of the sway")]
    public float swayAmplitude = 15f;
    [Tooltip("Sway frequency in Hz (cycles per second)")]
    public float swayFrequency = 1.5f;

    [Header("Cleanup")]
    [Tooltip("Destroy when y position falls below this")]
    public float destroyY = -300f;

    [Header("Hit Handling")]
    [Tooltip("Destroy the dart on contact (when hit by Dart)")]
    public bool destroyDartOnHit = true;

    private Rigidbody2D rb;
    private float t;
    private int dartLayer;
    private int playerLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        var col = GetComponent<Collider2D>();
        col.isTrigger = false; // using OnCollisionEnter2D

        dartLayer = LayerMask.NameToLayer("Dart");
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void FixedUpdate()
    {
        t += Time.fixedDeltaTime;

        float vx = sway ? Mathf.Sin(t * Mathf.PI * 2f * swayFrequency) * swayAmplitude : 0f;
        float vy = -fallSpeed;

        rb.velocity = new Vector2(vx, vy);

        if (transform.position.y < destroyY)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int otherLayer = collision.gameObject.layer;

        // React to Dart or Player
        if (otherLayer == dartLayer || otherLayer == playerLayer)
        {
            if (GameManager.Instance != null)
            {
                SoundManager.I.Play(SoundEvent.PlayerHit);
                GameManager.Instance.ResetRound();
            }

            if (otherLayer == dartLayer && destroyDartOnHit)
                Destroy(collision.gameObject);

            Destroy(gameObject);
        }
    }
}

