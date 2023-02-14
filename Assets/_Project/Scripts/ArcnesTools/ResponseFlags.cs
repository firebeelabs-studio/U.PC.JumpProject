using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseFlags
{
    public bool[] Flags;
    public bool UseDefaultCtr;

    public ResponseFlags()
    {
        if(UseDefaultCtr) return;
        
        ArcnesTools.Debug.Log("YOU DIDN'T USE PROPER CONSTRUCTOR, PLEASE MAKE SURE IT'S ON PURPOSE, IF YES SET 'UseDefaultCtr' AS TRUE ");
    }
    /// <summary>
    /// Use it when you wanna have multiple flags in one function
    /// </summary>
    /// <param name="flagsCount">Number of flags which you will be using</param>
    public ResponseFlags(int flagsCount)
    {
        Flags = new bool[flagsCount];
    }
    
    /// <summary>
    /// Checks if every flag is marked as true
    /// </summary>
    /// <returns></returns>
    public bool IsEverythingTrue()
    {
        if (Flags is null) return false;
        
        foreach (var flag in Flags)
        {
            if (flag == false) return false;
        }

        return true;
    }

    /// <summary>
    /// Marks every flag as reached
    /// </summary>
    public void MarkEverythingAsReached()
    {
        for (int i = 0; i < Flags.Length; i++)
        {
            Flags[i] = true;
        }
    }

    public void MarkNextFlagAsReached()
    {
        for (int i = 0; i < Flags.Length; i++)
        {
            if (!Flags[i])
            {
                Flags[i] = true;
                break;
            }
        }
    }
    
}