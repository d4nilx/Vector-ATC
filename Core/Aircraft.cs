using System;
using VectorATC.Core;

public class Aircraft
{
    public string Callsign { get; set; }
    public AircraftType Type { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Heading { get; set; }
    public float Speed { get; set; }
    public float Altitude { get; set; }
    public FlightPlan FlightPlan { get; set; }

    public Aircraft(string callsign, AircraftType type, float x, float y, 
        float heading, float speed, float altitude)
    {
        Callsign = callsign;
        Type = type;
        X = x;
        Y = y;
        Heading = heading;
        Speed = speed;
        Altitude = altitude;
    }

    public void Move(float deltaTime)
    {
        double radians = Heading * Math.PI / 180.0;
        X += (float)Math.Sin(radians) * Speed * deltaTime;
        Y -= (float)Math.Cos(radians) * Speed * deltaTime;
    }
}