namespace VectorATC.Core;

public class Waypoint
{
    public string Name { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    
    public Waypoint(string name, float x, float y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}