using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Alien", menuName = "Create new Alien")]
public class AlienScriptable : ScriptableObject
{
    public enum eAlienType { BaseAlien, ShieldAlien, SpecialAlien}
    public eAlienType alienType;

    public GameObject prefab;
    public int health;
    public int score;
    public int damage;
    public float minWaitTime;
    public float maxWaitTime;
    public float laserSpeed;
    public Color laserColor;
}
