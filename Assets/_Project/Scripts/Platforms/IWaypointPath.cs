using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaypointPath
{
    public Vector2[] Points { get; }
    public bool IsTrackLooped { get; }
}


