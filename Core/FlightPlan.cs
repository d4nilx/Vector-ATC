using System.Collections.Generic;

namespace VectorATC.Core;

public class FlightPlan
{
    public string DepartureIcao { get; set; }
    public string ArivalIcao { get; set; }
    public List<Waypoint> Route { get; set; }

    public FlightPlan(string departureIcao, string arivalIcao)
    {
        DepartureIcao = departureIcao;  
        ArivalIcao = arivalIcao;
        Route = new List<Waypoint>();
    }
}