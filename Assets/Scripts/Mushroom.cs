using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public Sprite[] states;
    private SpriteRenderer spriteRenderer;
    public int points = 1;
    private int health;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = states.Length;
    }

    public void Damage(int amount)
    {
        health -= amount;

        if (health > 0)
        {
            spriteRenderer.sprite = states[states.Length - health];
            SoundManager.I.Play(SoundEvent.MushroomHit);
        }
        else
        {
            GameManager.Instance.IncreaseScore(points);
            SoundManager.I.Play(SoundEvent.MushroomBreak);
            Destroy(gameObject);
        }
    }

    public void Heal()
    {
        health = states.Length;
        spriteRenderer.sprite = states[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            Damage(1);
        }
    }
}
