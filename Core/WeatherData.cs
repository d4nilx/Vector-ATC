namespace VectorATC.Core;

public class WeatherData
{
    public int WindSpeedKt { get; set; }
    public int WindDirection { get; set; }
    public double QnhHpa { get; set; }
    public int VisibilityM { get; set; }
    
    public WeatherData(int windSpeedKt, int windDirection, double qnhHpa, int visibilityM)
    {
        WindSpeedKt = windSpeedKt;
        WindDirection = windDirection;
        QnhHpa = qnhHpa;
        VisibilityM = visibilityM;
    }

    public override string ToString()
    {
        return $"Wind: {WindDirection}° at {WindSpeedKt} kt, QNH: {QnhHpa} hPa, Visibility: {VisibilityM} m";
    }
}