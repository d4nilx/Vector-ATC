using System;
using System.Collections.Generic;
using VectorATC.Core;

public class TrafficSpawner
{
    private float _width;
    private float _height;
    private float _timeSinceLastSpawn = 0f;
    private float _spawnInterval = 45f;
    private Random _random = new Random();
    private int _callsignCounter = 100;

    public TrafficSpawner()
    {
    }

    public void UpdateBounds(float width, float height)
    {
        _width = width;
        _height = height;
    }

    public void Update(float deltaTime, List<Aircraft> aircraftList)
    {
        if (_width == 0 || _height == 0) return;

        _timeSinceLastSpawn += deltaTime;
        if (_timeSinceLastSpawn >= _spawnInterval)
        {
            _timeSinceLastSpawn = 0f;
            aircraftList.Add(SpawnRandomAircraft());
        }
    }

    private Aircraft SpawnRandomAircraft()
    {
        var types = AircraftTypeDatabase.Common;
        var type = types[_random.Next(types.Count)];

        int edge = _random.Next(4);
        float startX, startY, heading;
        const float margin = 5f; 

        switch (edge)
        {
            case 0: 
                startX = (float)(_random.NextDouble() * _width);
                startY = margin;
                heading = 180;
                break;
            case 1: 
                startX = (float)(_random.NextDouble() * _width);
                startY = _height - margin;
                heading = 0;
                break;
            case 2: 
                startX = margin;
                startY = (float)(_random.NextDouble() * _height);
                heading = 90;
                break;
            default: 
                startX = _width - margin;
                startY = (float)(_random.NextDouble() * _height);
                heading = 270;
                break;
        }

        Array categories = Enum.GetValues(typeof(FlightCategory));
        FlightCategory randomCategory = (FlightCategory)categories.GetValue(_random.Next(categories.Length));
        
        var aircraft = new Aircraft(
            callsign: $"SQP{_callsignCounter++}",
            type: type,
            x: startX,
            y: startY,
            heading: heading,
    
            speed: _random.Next(200, 350), 
    
            altitude: 5000 + _random.Next(0, 30) * 100
        );
        
        aircraft.Category = randomCategory;

        var (exitX, exitY) = GetOppositePoint(edge);
        var plan = new FlightPlan("ENTRY", "EXIT");
        plan.Route.Add(new Waypoint("EXIT", exitX, exitY));
        aircraft.FlightPlan = plan;

        return aircraft;
    }

    private (float, float) GetOppositePoint(int entryEdge)
    {
        float jitter = 150f; // small random offset for exit point

        switch (entryEdge)
        {
            case 0: // entered from top -> exit bottom
                return ((float)(_random.NextDouble() * _width), _height + 50);
            case 1: // entered from bottom -> exit top
                return ((float)(_random.NextDouble() * _width), -50);
            case 2: // entered from left -> exit right
                return (_width + 50, (float)(_random.NextDouble() * _height));
            default: // entered from right -> exit left
                return (-50, (float)(_random.NextDouble() * _height));
        }
    }
}