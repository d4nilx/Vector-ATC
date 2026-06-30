using System.Collections.Generic;

public static class AircraftTypeDatabase
{
    public static List<AircraftType> Common = new List<AircraftType>
    {
        new AircraftType("B738", "Boeing", "737-800", 453, 140),
        new AircraftType("A320", "Airbus", "A320", 447, 135),
        new AircraftType("A21N", "Airbus", "A321neo", 450, 140),
        new AircraftType("E195", "Embraer", "E195", 450, 130),
    };
}