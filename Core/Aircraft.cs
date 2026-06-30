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
    
    private int _currentWaypointIndex = 0;

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
        if (FlightPlan != null && FlightPlan.Route.Count > 0)
        {
            MoveTowardsWaypoint(deltaTime);
        }
        else
        {
            MoveStraight(deltaTime);
        }
    }

    private void MoveTowardsWaypoint(float deltaTime)
    {
        if (_currentWaypointIndex >= FlightPlan.Route.Count)
        {
            return; // No more waypoints to follow
        }
        var target = FlightPlan.Route[_currentWaypointIndex];
        float dx = target.X - X;
        float dy = target.Y - Y;
        float distance = MathF.Sqrt(dx * dx + dy * dy);
        
        const float arrivalThreshold = 5.0f; // Distance threshold to consider waypoint reached
        if (distance < arrivalThreshold)
        {
            _currentWaypointIndex++;
            return;
        }
        
        // course 
        Heading = (float)(Math.Atan2(dx, -dy) * 180.0 / Math.PI);
        if (Heading < 0) Heading += 360;
        
        MoveStraight (deltaTime);
    }

    private void MoveStraight(float deltaTime)
    {
        double radians = Heading * Math.PI / 180;
        X += (float)Math.Sin(radians) * Speed * deltaTime;
        Y += (float)Math.Cos(radians) * Speed * deltaTime;
    }
}