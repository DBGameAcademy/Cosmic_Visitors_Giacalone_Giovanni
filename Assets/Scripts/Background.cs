using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Vector3 startPos;
    private float height;
    public float speed;

    private void Awake()
    {
        startPos = transform.position;
        height = GetComponent<BoxCollider2D>().size.y / 2;
    }
    private void Update()
    {
        if (transform.position.y < startPos.y - height)
        {
            transform.position = startPos;
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
