using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Create new Level")]
public class LevelScriptable : ScriptableObject
{
    public int levelNumber;
    public float moveDelay;

    [SerializeField][HideInInspector] private string row1;
    [SerializeField][HideInInspector] private string row2;
    [SerializeField][HideInInspector] private string row3;
    [SerializeField][HideInInspector] private string row4;
    [SerializeField][HideInInspector] private string row5;
    
    public string Row1 { get { return row1; } set { row1 = value; } }
    public string Row2 { get { return row2; } set { row2 = value; } }
    public string Row3 { get { return row3; } set { row3 = value; } }
    public string Row4 { get { return row4; } set { row4 = value; } }
    public string Row5 { get { return row5; } set { row5 = value; } }
}
