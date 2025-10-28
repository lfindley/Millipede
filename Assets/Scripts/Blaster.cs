using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Vector2 direction;
    private Vector2 spawnPosition;

    [Header("Movement")]
    public float speed = 20f;

    [Header("Shooting")]
    public GameObject dartPrefab;     // Assign in Inspector
    public Transform firePoint;       // Assign in Inspector
    public float fireCooldown = 0.2f; // Time between shots

    private float lastFireTime = 0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }

    private void Update()
    {
        // Movement input
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        // Shooting input
        if (Input.GetButton("Jump") && Time.time > lastFireTime + fireCooldown)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += direction.normalized * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position);
    }

    private void Shoot()
    {
        // Spawn the dart
        Instantiate(dartPrefab, firePoint.position, Quaternion.identity);

        // Play shoot SFX
        if (SoundManager.I != null)
            SoundManager.I.Play(SoundEvent.Shoot);

        lastFireTime = Time.time;
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
}
