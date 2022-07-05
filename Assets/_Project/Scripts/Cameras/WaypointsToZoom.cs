using System;

[Serializable]
public class WaypointsToZoom
{
    public int waypointNumber;
    public float zoomValue;
    public ZoomType zoomType;
    public enum ZoomType
    {
        ZoomIn,
        ZoomOut,
        ResetToDefault
    }
}
