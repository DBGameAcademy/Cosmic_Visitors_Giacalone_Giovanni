using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int speed;
    public int health = 15;
    public int Health { get { return health; } }
    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }
    //public bool isPlaying = false;
    private bool goingNextLevel;

    [SerializeField] private ParticleSystem particle;
    private ParticleSystem particleObj;
    private bool particlePlayed;

    private bool hasFired;
    private float fireCooldown;

    public int damage = 1;

    private void Awake()
    {
        speed = 7;
        //health = 15;
        currentHealth = health;

        UIManager.Instance.UpdateHealth(health, currentHealth);
        
        fireCooldown = .7f;
        //isPlaying = true;
        goingNextLevel = true;
        particlePlayed = false;
        hasFired = false;
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.eGameState.Playing)
        {
            goingNextLevel = true;
            if (!hasFired)
            {
                Fire();
            }
        }
        else if (GameManager.Instance.gameState == GameManager.eGameState.ResetLevel)
        {
            if (!particlePlayed)
            {
                particleObj = Instantiate(particle, transform.position - new Vector3(0, .5f, 0), transform.rotation * Quaternion.Euler(90,0,0));
                particleObj.transform.SetParent(transform);
                particlePlayed = true;
                hasFired = false;
            }
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
        transform.Translate(Vector3.right * speed * hInput * Time.fixedDeltaTime);
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
            transform.Translate(Vector3.up * Time.fixedDeltaTime * speed * 2);
            return;
        }

        particlePlayed = false;
        Destroy(particleObj);
        transform.position = new Vector3(0, -4f, 0);
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
        UIManager.Instance.UpdateHealth(health, currentHealth);
        EventManager.Instance.TriggerEvent("OnPlayerHit");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            EventManager.Instance.TriggerEvent("OnPlayerDead");
        }
    }

    private void Fire()
    {
        hasFired = true;
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject laser = LaserFactory.Instance.CreateObject("PlayerLaser", position, Quaternion.identity);
        laser.GetComponent<Laser>().damage = damage;
        laser.GetComponent<Laser>().speed = 10;
        GameManager.Instance.AddLaser(laser);
        StartCoroutine(ResetFireCoroutine());
    }

    IEnumerator ResetFireCoroutine()
    {
        yield return new WaitForSeconds(fireCooldown);
        hasFired = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(collision.gameObject.GetComponent<Laser>().damage);
        LaserFactory.Instance.ReturnObject(collision.gameObject);
    }
}
