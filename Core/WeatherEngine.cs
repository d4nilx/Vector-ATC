using System;
using VectorATC.Core;

public class WeatherEngine
{
    private Random _random = new Random();
    public WeatherData Current { get; private set; }

    public WeatherEngine()
    {
        Current = new WeatherData(10, 270, 1013, 9999);
    }

    public void Randomize()
    {
        Current.WindDirection = _random.Next(0, 360);
        Current.WindSpeedKt = _random.Next(0, 35);
        Current.QnhHpa = 980 + _random.Next(0, 60);
        Current.VisibilityM = _random.Next(0, 4) == 0
            ? _random.Next(500, 5000)   
            : 9999;
        Current.CloudCoverageOktas = _random.Next(0, 9);
        Current.CloudBaseft = 500 + _random.Next(0, 50) * 100;
    }
}