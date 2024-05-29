using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float speed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float attackCooldown = 0.2f;
    private float _lastAttackTime = 0;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(Time.timeScale == 0) return;
        HandleRotation();
        HandleMovement();
        HandleAttack();
    }
    
    void HandleMovement()
    {
        // Get the horizontal and vertical input from the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Create a new Vector3 to store the movement direction
        Vector3 movement = new Vector3(horizontal, vertical, 0);

        // Normalize the movement vector to ensure that the player moves at a consistent speed
        movement.Normalize();

        // Move the player in the direction of the movement vector
        transform.position += movement * (Time.deltaTime * speed);
    }

    void HandleRotation()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, mainCamera.nearClipPlane));

        // Calculate the direction from the player to the mouse position
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0; 

        // Calculate the angle between the player's forward vector and the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the player to face the mouse cursor
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }
    
    void HandleAttack()
    {
        if (Input.GetMouseButton(0) && Time.time - _lastAttackTime > attackCooldown)
        {
            _lastAttackTime = Time.time;
            // Create a new bullet object
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            // get the direction from the player to the mouse cursor
            Vector3 direction = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.z = 0;
            direction.Normalize();
            bullet.GetComponent<BulletMovement>().Initialize(direction, bulletSpeed);
        }
    } 
}
