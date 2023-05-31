using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Alien Collection", menuName = "Create new Alien Collection")]
public class AlienCollection : ScriptableObject
{
    public List<AlienScriptable> alienCollection = new List<AlienScriptable>();

    public AlienScriptable RandomAlien()
    {
        AlienScriptable newAlien = alienCollection[Random.Range(0, alienCollection.Count)];
        return newAlien;
    }
}
