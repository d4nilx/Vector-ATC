namespace VectorATC.Core;

public class WeatherData
{
    public int WindDirection { get; set; }    
    public int WindSpeedKt { get; set; }      
    public int QnhHpa { get; set; }           
    public int VisibilityM { get; set; }      
    public int CloudCoverageOktas { get; set; } 
    public int CloudBaseft { get; set; }
    
    public WeatherData(int windSpeedKt, int windDirection, double qnhHpa, int visibilityM)
    {
        WindDirection = 270;
        WindSpeedKt = 10;
        QnhHpa = 1013;
        VisibilityM = 9999;
        CloudCoverageOktas = 2;
        CloudBaseft = 3000;
    }

    public string ToMetarString()
    {
        string wind = $"{WindDirection:000}/{WindSpeedKt:00}kt";
        string clouds = CloudCoverageOktas == 0 ? "SKC" :
                        CloudCoverageOktas <= 2 ? $"FEW{CloudBaseft / 100:000}" :
                        CloudCoverageOktas <= 4 ? $"SCT{CloudBaseft / 100:000}" : 
                        CloudCoverageOktas <= 7 ? $"BKN{CloudBaseft / 100:000}" :
                                                  $"OVC{CloudBaseft / 100:000}";
        string vis = VisibilityM >= 9999 ? "9999" : $"{VisibilityM}m";
        return $"EPWA {wind}  Q{QnhHpa}  {vis}  {clouds}";
    }
}