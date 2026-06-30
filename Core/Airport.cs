using System.Collections.Generic;

namespace VectorATC.Core;

public class Airport
{
    public string Icao { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public List<Waypoint> Waypoints { get; set; }

    public Airport(string icao, string name)
    {
        Icao = icao;
        Name = name;
        Country = string.Empty;
        Waypoints = new List<Waypoint>();
    }
}