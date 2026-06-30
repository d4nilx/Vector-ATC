using System;
using System.Collections.Generic;
using VectorATC.Core;

public class TrafficSpawner
{
    private float _width;
    private float _height;
    private float _timeSinceLastSpawn = 0f;
    private float _spawnInterval = 5f;
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

        // Choose a random edge of the screen for the aircraft to spawn from
        int edge = _random.Next(4);
        float startX = 0, startY = 0, heading = 0;

        // Random for plane course variance to make it more natural
        int variance = _random.Next(-30, 30);

        switch (edge)
        {
            case 0: // From the top goin down
                startX = (float)(_random.NextDouble() * _width);
                startY = -50; 
                heading = 180 + variance;
                break;
            case 1: // From the bottom going up
                startX = (float)(_random.NextDouble() * _width);
                startY = _height + 50;
                heading = 0 + variance;
                if (heading < 0) heading += 360; 
                break;
            case 2: // From the left, flying right
                startX = -50;
                startY = (float)(_random.NextDouble() * _height);
                heading = 90 + variance;
                break;
            default: // From the right, flying left
                startX = _width + 50;
                startY = (float)(_random.NextDouble() * _height);
                heading = 270 + variance;
                break;
        }

        var aircraft = new Aircraft(
            callsign: $"SQP{_callsignCounter++}",
            type: type,
            x: startX,
            y: startY,
            heading: heading,
            speed: 60, 
            altitude: 5000 + _random.Next(0, 30) * 100
        );

        // Set a simple flight plan to exit the screen on the opposite side
        var plan = new FlightPlan("ENTRY", "EXIT");
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