using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Create new Level")]
public class LevelScriptable : ScriptableObject
{
    public int levelNumber;
    public float moveDelay;

    [SerializeField] private string row1;
    [SerializeField] private string row2;
    [SerializeField] private string row3;
    [SerializeField] private string row4;
    [SerializeField] private string row5;
    
    public string Row1 { get; set; }
    public string Row2 { get; set; }
    public string Row3 { get; set; }
    public string Row4 { get; set; }
    public string Row5 { get; set; }
}
