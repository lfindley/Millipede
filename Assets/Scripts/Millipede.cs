using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Millipede : MonoBehaviour
{
    private readonly List<MillipedeSegment> segments = new List<MillipedeSegment>();

    [Header("Prefabs")]
    public MillipedeSegment segmentPrefab;
    public Mushroom mushroomPrefab;

    [Header("Sprites")]
    public Sprite headSprite;
    public Sprite bodySprite;

    [Header("Movement")]
    public LayerMask collisionMask = ~0;
    public Collider2D homeArea;
    public float speed = 20f;
    public int size = 12;

    [Header("Scoring")]
    public int pointsHead = 100;
    public int pointsBody = 10;

    private void OnEnable()
    {
        foreach (MillipedeSegment segment in segments)
        {
            segment.enabled = true;
        }
    }

    private void OnDisable()
    {
        foreach (MillipedeSegment segment in segments)
        {
            segment.enabled = true;
        }
    }

    public void Remove(MillipedeSegment segment)
    {
        int points = segment.isHead ? pointsHead : pointsBody;
        GameManager.Instance.IncreaseScore(points);

        Vector2 position = GridPosition(segment.transform.position);
        Instantiate(mushroomPrefab, position, Quaternion.identity);

        if (segment.ahead != null)
        {
            segment.ahead.behind = null;
        }

        if (segment.behind != null)
        {
            segment.behind.ahead = null;
            segment.behind.UpdateHeadSegment();
        }

        segments.Remove(segment);
        Destroy(segment.gameObject);

        if (segments.Count == 0)
        {
            GameManager.Instance.NextLevel();
        }
    }

    public void Respawn(int size)
    {
        foreach (MillipedeSegment segment in segments)
        {
            Destroy(segment.gameObject);
        }

        segments.Clear();

        for (int i = 0; i < size; i++)
        {
            Vector2 position = GridPosition(transform.position) + (Vector2.left * i);
            MillipedeSegment segment = Instantiate(segmentPrefab, position, Quaternion.identity, transform);
            segment.spriteRenderer.sprite = i == 0 ? headSprite : bodySprite;
            segment.millipede = this;
            segment.enabled = enabled;
            segments.Add(segment);
        }

        for (int i = 0; i < segments.Count; i++)
        {
            MillipedeSegment segment = segments[i];
            segment.ahead = GetSegmentAt(i - 1);
            segment.behind = GetSegmentAt(i + 1);
        }

        enabled = true;
    }

    private MillipedeSegment GetSegmentAt(int index)
    {
        if (index >= 0 && index < segments.Count)
        {
            return segments[index];
        }
        else
        {
            return null;
        }
    }

    private Vector2 GridPosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

}
