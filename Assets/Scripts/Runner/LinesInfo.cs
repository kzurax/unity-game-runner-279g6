using System;
using UnityEngine;

[Serializable]
public class LinesInfo
{
    [SerializeField] private int   LinesCount;
    [SerializeField] private float LineOffset;
    
    //public Alignment align = center

    public int LineDefault => Mathf.FloorToInt(LinesCount / 2f);
    public int Min => 0;
    public int Max => LinesCount-1;
    
    public Vector3 GetLineOffset(int line) => new(LineOffset * (line - (LinesCount - 1) / 2f), 0, 0);
}
