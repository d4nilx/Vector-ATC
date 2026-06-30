public class AircraftType
{
    public string IcaoCode { get; set; }      // for instance B-737
    public string Manufacturer { get; set; }  // for instance Boeing
    public string Model { get; set; }         // for instance 737-800
    public float CruiseSpeedKt { get; set; }  // for instance 450
    public float ApproachSpeedKt { get; set; } // for instance 130

    public AircraftType(string icaoCode, string manufacturer, string model, 
        float cruiseSpeedKt, float approachSpeedKt)
    {
        IcaoCode = icaoCode;
        Manufacturer = manufacturer;
        Model = model;
        CruiseSpeedKt = cruiseSpeedKt;
        ApproachSpeedKt = approachSpeedKt;
    }

    public override string ToString() => $"{Manufacturer} {Model} ({IcaoCode})";
}