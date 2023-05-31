using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int speed;
    private int health;
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }
    public bool isPlaying = false;
    public bool goingNextLevel;
    //private bool particlePlayed;
    //private bool hasFired;
    private float fireCooldown;

    private void Awake()
    {
        speed = 7;
        health = 5;
        currentHealth = health;
        //UIManager.Instance.UpdateLifes(currentHealth);
        fireCooldown = .7f;
        isPlaying = true;
        goingNextLevel = true;
        //particlePlayed = false;
        //hasFired = false;
    }

    private void OnEnable()
    {
        //EventManager.Instance.StartListening("OnLevelCompleted", InvokeReset);
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.eGameState.Playing)
        {
            goingNextLevel = true;
            Fire();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameState == GameManager.eGameState.Playing)
        {
            Move();
        }
        else if (GameManager.Instance.gameState == GameManager.eGameState.ResetLevel)
        {
            Invoke("ResetPosition", 1);
        }
    }

    private void Move()
    {
        float bound = 8.387501f;
        float hInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * speed * hInput * Time.deltaTime);
        if (transform.position.x > bound)
        {
            transform.position = new Vector3(bound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -bound)
        {
            transform.position = new Vector3(-bound, transform.position.y, transform.position.z);
        }
    }

    private void ResetPosition()
    {

        while (transform.position.y < 7 && goingNextLevel)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed * 10);
            return;
        }

        //particlePlayed = false;
        //Destroy(particle);
        transform.position = new Vector3(0, -4.5f, 0);
        goingNextLevel = false;

        EventManager.Instance.TriggerEvent("OnPlayerReset");
    }

    private void TakeDamage(int _damage)
    {
        if (_damage >= currentHealth)
        {
            _damage = currentHealth;
        }

        currentHealth -= _damage;
        //UIManager.Instance.UpdateLifes(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            EventManager.Instance.TriggerEvent("OnPlayerDead");
        }
    }

    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            GameObject laser = LaserFactory.Instance.CreateObject("PlayerLaser", position, Quaternion.identity);
            laser.GetComponent<Laser>().damage = 1;
            laser.GetComponent<Laser>().speed = 10;
            // add laser in a list (?)
        }
    }

    IEnumerator ResetFireCoroutine()
    {
        yield return new WaitForSeconds(fireCooldown);
        //hasFired = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(collision.gameObject.GetComponent<Laser>().damage);
        LaserFactory.Instance.ReturnObject(collision.gameObject);
    }
}
