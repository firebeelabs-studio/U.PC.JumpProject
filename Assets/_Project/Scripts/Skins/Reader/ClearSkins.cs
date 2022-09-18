using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSkins : MonoBehaviour
{
    public void Clear()
    {
        SkinsHolder.Instance.ClearSkins();
    }
}
