using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienManager : Helper.MonoSingleton<AlienManager>
{
    public List<Alien> aliensInGame;
    public int descendingIndex;
    public float moveSpace;
    public float downSpace;
    public float moveDelay;
    private bool canMove = true;
    [SerializeField]
    private bool goingRight = true;
    public Alien alienMostRight;
    public Alien alienMostLeft;

    private void OnEnable()
    {
        EventManager.Instance.StartListening("OnCollisionHappened", ChangeDirection);
    }

    private void Start()
    {
        alienMostRight = null;
        alienMostLeft = null;
        SetupLevel();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.eGameState.Playing)
        {
            Move();
            if (aliensInGame.Count == 0)
            {
                EventManager.Instance.TriggerEvent("OnLevelCompleted");
            }
        }

    }

    private void OnDisable()
    {
        EventManager.Instance.StopListening("OnCollisionHappened", ChangeDirection);
    }

    public void SetupLevel()
    {
        moveDelay = LevelManager.Instance.moveDelay;
        if (moveDelay <= .8f)
        {
            moveDelay = .8f;
        }
    }
    
    private void Move()
    {
        FindMostRightAndLeftAlien();
        
        if (canMove)
        {
            canMove = false;
            
            switch(goingRight)
            {
                case true:
                    if (CanIMoveRightRaycast(alienMostRight) && alienMostRight != null)
                    {
                        transform.position += new Vector3(moveSpace, 0, 0);
                    }
                    else if (!CanIMoveRightRaycast(alienMostRight) && alienMostRight != null)
                    {
                        ChangeDirection();
                    }
                    break;
                
                case false:
                    if (CanIMoveLeftRaycast(alienMostLeft) && alienMostLeft != null)
                    {
                        transform.position -= new Vector3(moveSpace, 0, 0);
                    }
                    else if (!CanIMoveLeftRaycast(alienMostLeft) && alienMostLeft != null)
                    {
                        ChangeDirection();
                    }
                    break;
            }
            StartCoroutine(ResetMove(moveDelay));
        }
    }

    IEnumerator ResetMove(float _speed)
    {
        if (descendingIndex <= 2)
        {
            yield return new WaitForSeconds(_speed);
        }
        else if (descendingIndex > 2 && descendingIndex <= 4)
        {
            _speed -= .3f;
            yield return new WaitForSeconds(_speed);
        }
        else if (descendingIndex > 4 && descendingIndex <= 7)
        {
            _speed -= .5f;
            yield return new WaitForSeconds(_speed);
        }
        else if (descendingIndex > 7)
        {
            _speed -= .7f;
            yield return new WaitForSeconds(_speed);
        }
        canMove = true;
    }

    public void ChangeDirection()
    {
        goingRight = goingRight ? false : true;
        transform.position -= new Vector3(0, downSpace, 0);
        descendingIndex++;
    }

    public void FindMostRightAndLeftAlien()
    {
        float maxX = -Mathf.Infinity;
        float minX = -Mathf.Infinity;
        for (int i = aliensInGame.Count - 1; i >= 0; i--)
        {
            float x = aliensInGame[i].transform.position.x;
            if (x > maxX)
            {
                maxX = x;
                alienMostRight = aliensInGame[i];
            }
            
            float y = aliensInGame[i].transform.position.x;
            if (y < -minX)
            {
                minX = y;
                alienMostLeft = aliensInGame[i];
            }
        }
    }

    public bool CanIMoveRightRaycast(Alien _targetAlien)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(_targetAlien.transform.position, Vector2.right);
        float distance = Mathf.Abs(hits[hits.Length - 1].distance) - _targetAlien.colliderSize;
        if (distance >= moveSpace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CanIMoveLeftRaycast(Alien _targetAlien)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(_targetAlien.transform.position, Vector2.left);
        float distance = Mathf.Abs(hits[hits.Length - 1].distance) - _targetAlien.colliderSize;
        if (distance >= moveSpace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
