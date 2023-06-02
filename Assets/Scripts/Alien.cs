using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour, IPoolable
{
    private int health;
    private int currentHealth;
    private int score;
    private float minWaitTime;
    private float maxWaitTime;
    private int laserDamage;
    private float laserSpeed;
    private Color laserColor;
    private bool hasFired;
    private PolygonCollider2D polyCollider;
    public float colliderSize;
    private bool isAlive;
    public bool IsAlive
    {
        get => isAlive;
        set => isAlive = value;
    }

    private Color originalColor;

    private void Awake()
    {
        hasFired = true;
        polyCollider = GetComponent<PolygonCollider2D>();
        colliderSize = polyCollider.bounds.size.x / 2;
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
        hasFired = false;
        SetSpriteColor(originalColor);
    }

    IEnumerator FireCoroutine(float _minWaitTime, float _maxWaitTime)
    {
        hasFired = true;
        Vector3 position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        GameObject laser = LaserFactory.Instance.CreateObject("Laser", position, Quaternion.identity);
        
        laser.GetComponent<Laser>().damage = laserDamage;
        laser.GetComponent<Laser>().speed = laserSpeed;
        laser.GetComponent<SpriteRenderer>().color = laserColor;
        GameManager.Instance.AddLaser(laser); 
       
        yield return new WaitForSeconds(Random.Range(_minWaitTime, _maxWaitTime));
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
            UIManager.Instance.UpdateScore(score);
        }
    }

    private void ManageCollection(Alien _alien)
    {
        AlienManager.Instance.aliensInGame.Remove(_alien);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("AlienLaser"))
        {
            TakeDamage(collision.gameObject.GetComponent<Laser>().damage);
            LaserFactory.Instance.ReturnObject(collision.gameObject);
        }
    }


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
