using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienFactory : Helper.MonoSingleton<AlienFactory>
{
    [SerializeField]
    private AlienCollection collection;
    private int poolSize = 10;
    private List<Alien> pool;
    private List<Alien> currentlyActive;
    private Dictionary<Alien, AlienScriptable> alienDictionary = new Dictionary<Alien, AlienScriptable>();

    private void Start()
    {
        pool = new List<Alien>();
        currentlyActive = new List<Alien>();

        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(AllocateAlien());
        }
    }

    private void Update()
    {
        ManagePool();
    }

    public Alien Create()
    {
        if (pool.Count == 0)
        {
            pool.Add(AllocateAlien());
            Debug.Log("Adding alien to the pool!");
        }

        Alien alien = pool[0];
        alien.gameObject.SetActive(true);
        alien.IsAlive = true;

        pool.Remove(alien);
        currentlyActive.Add(alien);

        return alien;
    }

    public Alien Create(Vector3 _position)
    {
        if (pool.Count == 0)
        {
            pool.Add(AllocateAlien());
        }

        Alien alien = pool[0];
        alien.gameObject.SetActive(true);
        alien.IsAlive = true;
        alien.transform.position = _position;

        alien.SetUp(alienDictionary[alien]);
        pool.Remove(alien);
        currentlyActive.Add(alien);

        return alien;
    }

    public Alien Create(Vector3 _position, Quaternion _rotation)
    {
        if (pool.Count == 0)
        {
            pool.Add(AllocateAlien());
            Debug.Log("Adding alien to the pool!");
        }

        Alien alien = pool[0];
        alien.gameObject.SetActive(true);
        alien.IsAlive = true;
        alien.SetUp(alienDictionary[alien]);
        alien.transform.position = _position;
        alien.transform.rotation = _rotation;

        pool.Remove(alien);
        currentlyActive.Add(alien);

        return alien;
    }

    private void ManagePool()
    {
        for (int i = currentlyActive.Count - 1; i >= 0; i--)
        {
            Alien alien = currentlyActive[i];
            if (!alien.IsAlive)
            {
                alien.gameObject.SetActive(false);
                currentlyActive.Remove(alien);
                pool.Add(alien);
            }
        }
    }

    private Alien AllocateAlien()
    {
        AlienScriptable randAlien = collection.RandomAlien();
        GameObject alienObj = Instantiate(randAlien.prefab);
        Alien alien = alienObj.GetComponent<Alien>();
        alien.SetUp(randAlien);
        alien.gameObject.SetActive(false);

        alienDictionary.Add(alien, randAlien);
        return alien;
    }

    //public void Kill()
    //{
    //    if (currentlyActive.Count > 0)
    //    {
    //        currentlyActive.Peek().IsAlive = false;
    //    }
    //}
}
