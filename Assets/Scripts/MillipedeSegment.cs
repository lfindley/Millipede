using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillipedeSegment : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public BoxCollider2D boxCollider { get; private set; }

    public Millipede millipede { get; set; }
    public MillipedeSegment ahead { get; set; }
    public MillipedeSegment behind { get; set; }
    public bool isHead => ahead == null;

    private Vector2 direction = Vector2.right + Vector2.down;
    private Vector2 targetPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        targetPosition = transform.position;
    }

    private void OnEnable()
    {
        boxCollider.enabled = true;
    }

    private void OnDisable()
    {
        boxCollider.enabled = false;
    }

    private void Update()
    {
        // Set the next target position if the segment has reached its target
        if (isHead && Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            UpdateHeadSegment();
        }

        // Move towards the target position
        Vector2 currentPosition = transform.position;
        float speed = millipede.speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed);

        // Rotate the segment to face the direction it is moving
        Vector2 movementDirection = (targetPosition - currentPosition).normalized;
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

        // Set the corresponding sprite
        spriteRenderer.sprite = isHead ? millipede.headSprite : millipede.bodySprite;
    }

    public void UpdateHeadSegment()
    {
        Vector2 gridPosition = GridPosition(transform.position);

        // Calculate the next grid position
        targetPosition = gridPosition;
        targetPosition.x += direction.x;

        // Check if the segment will collide with an object
        if (Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, millipede.collisionMask))
        {
            // Reverse horizontal direction
            direction.x = -direction.x;

            // Advance to the next row
            targetPosition.x = gridPosition.x;
            targetPosition.y = gridPosition.y + direction.y;

            Bounds homeBounds = millipede.homeArea.bounds;

            // Reverse vertical direction if the segment leaves the home area
            if ((direction.y == 1f && targetPosition.y > homeBounds.max.y) ||
                (direction.y == -1f && targetPosition.y < homeBounds.min.y))
            {
                direction.y = -direction.y;
                targetPosition.y = gridPosition.y + direction.y;
            }
        }

        // Update the body segments
        if (behind != null)
        {
            behind.UpdateBodySegment();
        }
    }

    private void UpdateBodySegment()
    {
        // Follow the segment ahead
        targetPosition = GridPosition(ahead.transform.position);
        direction = ahead.direction;

        // Update the next body segment
        if (behind != null)
        {
            behind.UpdateBodySegment();
        }
    }

    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && millipede.enabled)
        {
            millipede.enabled = false;
            GameManager.Instance.ResetRound();
            SoundManager.I.Play(SoundEvent.PlayerHit);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart") && millipede.enabled)
        {
            collision.collider.enabled = false;
            millipede.Remove(this);
            SoundManager.I.Play(SoundEvent.EnemyHit);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetPosition, Vector3.one);
    }
}
