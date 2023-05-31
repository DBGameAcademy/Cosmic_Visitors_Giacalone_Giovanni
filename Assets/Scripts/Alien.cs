using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour, IPoolable
{
    private int health;
    private int currentHealth;
    public int score;
    public float minWaitTime;
    public float maxWaitTime;
    public int laserDamage;
    public float laserSpeed;
    public Color laserColor;
    private bool hasFired;
    public PolygonCollider2D polyCollider;
    public float colliderSize;
    private bool isAlive;
    public bool IsAlive
    {
        get => isAlive;
        set => isAlive = value;
    }

    private void Awake()
    {
        hasFired = true;
        polyCollider = GetComponent<PolygonCollider2D>();
        colliderSize = polyCollider.bounds.size.x / 2;
        Invoke("StartFire", Random.Range(2, 10));
        originalColor = GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.eGameState.Playing)
        {
            if (!hasFired)
            {
                StartCoroutine(FireCoroutine(minWaitTime, maxWaitTime));
            }
        }
    }

    public void SetUp(AlienScriptable _prototype)
    {
        health = _prototype.health;
        score = _prototype.score;
        currentHealth = health;
        minWaitTime = _prototype.minWaitTime;
        maxWaitTime = _prototype.maxWaitTime;
        laserDamage = _prototype.damage;
        laserSpeed = _prototype.laserSpeed;
        laserColor = _prototype.laserColor;
    }

    IEnumerator FireCoroutine(float _minWaitTime, float _maxWaitTime)
    {
        hasFired = true;
        Vector3 position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        GameObject laser = LaserFactory.Instance.CreateObject("Laser", position, Quaternion.identity);
        
        // this should be handled with an event callback that can pass parameters
        laser.GetComponent<Laser>().damage = laserDamage;
        laser.GetComponent<Laser>().speed = laserSpeed;
        laser.GetComponent<SpriteRenderer>().color = laserColor;
        
        // add laser in a list (?)
        yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
        hasFired = false;
    }

    private void StartFire()
    {
        hasFired = false;
    }

    private void TakeDamage(int _damage)
    {
        if (_damage >= currentHealth)
        {
            _damage = currentHealth;
        }

        StartCoroutine(FlashWhite());
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            ManageCollection(this);
            IsAlive = false;
            // update the score
            // instantiate the particle effect
        }
    }

    private void ManageCollection(Alien _alien)
    {
        AlienManager.Instance.aliensInGame.Remove(_alien);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EventManager.Instance.TriggerEvent("OnCollisionHappened");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("AlienLaser"))
        {
            TakeDamage(collision.gameObject.GetComponent<Laser>().damage);
            LaserFactory.Instance.ReturnObject(collision.gameObject);
        }
    }

    private Color originalColor;

    private void SetSpriteColor(Color _color)
    {
        GetComponent<SpriteRenderer>().color = _color;
    }

    IEnumerator FlashWhite()
    {
        for (int n = 0; n < 2; n++)
        {
            SetSpriteColor(Color.white);
            yield return new WaitForSeconds(0.1f);
            SetSpriteColor(originalColor);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
