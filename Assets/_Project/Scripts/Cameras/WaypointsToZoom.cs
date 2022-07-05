using System;

[Serializable]
public class WaypointsToZoom
{
    public int waypointNumber;
    public float zoomValue;
    public ZoomType zoomType;

    public WaypointsToZoom(int waypointNumber, float zoomValue, ZoomType zoomType)
    {
        this.waypointNumber = waypointNumber;
        this.zoomValue = zoomValue;
        this.zoomType = zoomType;
    }

    public enum ZoomType
    {
        ZoomIn,
        ZoomOut,
        ResetToDefault
    }
}
