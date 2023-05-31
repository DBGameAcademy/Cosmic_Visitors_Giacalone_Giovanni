using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage;
    public float speed;
    public bool isEnemy;
    public bool isBoss;

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameState == GameManager.eGameState.Playing)
        {
            LaserMovement();
        }
    }

    private void LaserMovement()
    {
        if (!isEnemy)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            if (transform.position.y > 5)
            {
                LaserFactory.Instance.ReturnObject(gameObject);
            }
        }
        else if (isEnemy)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            if (transform.position.y < -6)
            {
                LaserFactory.Instance.ReturnObject(gameObject);
            }

        }
    }
}
