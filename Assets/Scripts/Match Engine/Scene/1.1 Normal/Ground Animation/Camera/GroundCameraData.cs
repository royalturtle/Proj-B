using UnityEngine;

public class GroundCameraData
{
    public Vector3 Position {get; private set;}
    public float Size {get; private set;}

    public GroundCameraData(float x=0.0f, float y=0.0f, float z=-10.0f, float size=5.0f)
    {
        Position = new Vector3(x, y, z);
        Size = size;
    }
}
