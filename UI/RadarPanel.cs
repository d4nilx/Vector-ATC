using Godot;
using System;
using System.Collections.Generic;
using VectorATC.Core;
using VectorATC.UI;

public partial class RadarPanel : Control
{
    public Main Main { get; set; }
    
    private List<Aircraft> _aircraft = new List<Aircraft>();
    private TrafficSpawner _spawner;
    private FastNoiseLite _weatherNoise;
    private Vector2 _noiseOffset = Vector2.Zero; 
    private int _previousAircraftCount = 0;
    public Aircraft SelectedAircraft { get; private set; }
    public event Action<Aircraft> OnAircraftSelected;
    public event Action<List<Aircraft>> OnTrafficUpdated;
    private SimulationClock _clock = new SimulationClock();

    public override void _Ready()
    {
        SizeFlagsVertical = SizeFlags.ExpandFill;
        CustomMinimumSize = new Vector2(0, 300);
        ClipContents = true;

        _spawner = new TrafficSpawner(); 
        GuiInput += OnGuiInput;
        
        _weatherNoise = new FastNoiseLite();
        _weatherNoise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        _weatherNoise.Seed = (int)GD.Randi(); 
        _weatherNoise.Frequency = 0.003f;     
    }
    
    public override void _Process(double delta)
    {
        if (Main != null)
        {
            var w = Main.Weather;
            if (w.WindSpeedKt > 0)
            {
                float blowRad = (float)((w.WindDirection - 180) * Math.PI / 180.0);
                Vector2 windDir = new Vector2(MathF.Sin(blowRad), -MathF.Cos(blowRad));
                _noiseOffset += windDir * w.WindSpeedKt * (float)delta * 0.5f;
            }
        }

        _spawner.UpdateBounds(Size.X, Size.Y);
        _spawner.Update((float)delta, _aircraft);

        if (_aircraft.Count != _previousAircraftCount)
        {
            _previousAircraftCount = _aircraft.Count;
            OnTrafficUpdated?.Invoke(_aircraft);
        }

        if (_clock.ShouldUpdate((float)delta))
        {
            foreach (var plane in _aircraft)
            {
                plane.Move(_clock.UpdateIntervalSeconds);
                plane.RecordTrailPosition();
            }
            QueueRedraw();
        }
        
        _aircraft.RemoveAll(p =>
            p.X < -50 || p.X > Size.X + 50 ||
            p.Y < -50 || p.Y > Size.Y + 50);
    }

    public override void _Draw()
    {
        var bgColor = new Color("#092d38");
        DrawRect(new Rect2(Vector2.Zero, Size), bgColor);

        DrawWeatherRadar();

        Vector2 center = Size / 2;
        var runwayColor = new Color("#5a7d87");
        DrawLine(center + new Vector2(-40, -10), center + new Vector2(40, 10), runwayColor, 2f); 
        DrawLine(center + new Vector2(-20, 30), center + new Vector2(20, -30), runwayColor, 2f); 
        DrawString(ThemeDB.FallbackFont, center + new Vector2(-55, -20), "EPWA", HorizontalAlignment.Left, -1, 14, Colors.LightGray);

        foreach (var plane in _aircraft)
        {
            for (int i = 0; i < plane.TrailPositions.Count; i++)
            {
                var (tx, ty) = plane.TrailPositions[i];
                float alpha = (float)(i + 1) / plane.TrailPositions.Count * 0.5f; 
                var trailColor = new Color("#00ffcc", alpha);
                DrawRect(new Rect2(tx - 2, ty - 2, 4, 4), trailColor);
            }
            
            var pos = new Vector2(plane.X, plane.Y);
            
            bool isSelected = plane == SelectedAircraft;
            Color baseColor = Colors.Gray; 
            if (plane.Category == FlightCategory.Arrival) baseColor = Colors.LimeGreen;
            if (plane.Category == FlightCategory.Departure) baseColor = Colors.Yellow;

            var radarColor = isSelected ? Colors.White : baseColor;
            float blipSize = isSelected ? 5f : 3f;

            DrawRect(new Rect2(pos.X - blipSize, pos.Y - blipSize, blipSize * 2, blipSize * 2), radarColor);

            if (isSelected)
                DrawArc(pos, 14f, 0, MathF.Tau, 16, radarColor, 1f);

            var textAnchor = pos + new Vector2(20, -20);
            DrawLine(pos, textAnchor, radarColor, 1f);

            DrawString(ThemeDB.FallbackFont, textAnchor + new Vector2(5, -2),
                plane.Callsign, HorizontalAlignment.Left, -1, 14, radarColor);

            string altStr = ((int)(plane.Altitude / 100)).ToString("D3");
            string spdStr = ((int)plane.Speed).ToString("D3");
            DrawString(ThemeDB.FallbackFont, textAnchor + new Vector2(5, 14),
                $"{altStr}={spdStr}", HorizontalAlignment.Left, -1, 14, radarColor);
        }
        
        if (Main != null)
        {
            var w = Main.Weather;
            var windOrigin = new Vector2(60, Size.Y - 60); 
            float windRad = (float)(w.WindDirection * Math.PI / 180.0);
            float lineLength = 35f; 
        
            var windEnd = windOrigin + new Vector2(
                MathF.Sin(windRad),
                -MathF.Cos(windRad)
            ) * lineLength;

            var radarColor = new Color("#00ffcc");
            DrawLine(windOrigin, windEnd, radarColor, 2f);

            float arrowHeadLength = 10f; 
            float arrowAngle = 0.5f;     
        
            float leftAngle = windRad + MathF.PI - arrowAngle;
            var leftPoint = windEnd + new Vector2(MathF.Sin(leftAngle), -MathF.Cos(leftAngle)) * arrowHeadLength;
        
            float rightAngle = windRad + MathF.PI + arrowAngle;
            var rightPoint = windEnd + new Vector2(MathF.Sin(rightAngle), -MathF.Cos(rightAngle)) * arrowHeadLength;

            DrawLine(windEnd, leftPoint, radarColor, 2f);
            DrawLine(windEnd, rightPoint, radarColor, 2f);

            DrawString(ThemeDB.FallbackFont, windOrigin + new Vector2(-25, 25),
                $"{w.WindDirection:000}/{w.WindSpeedKt:00}kt",
                HorizontalAlignment.Left, -1, 14, radarColor);
        }
    }
    
    private void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouse &&
            mouse.Pressed &&
            mouse.ButtonIndex == MouseButton.Left)
        {
            const float clickRadius = 12f;
            Aircraft closest = null;
            float closestDist = float.MaxValue;

            foreach (var plane in _aircraft)
            {
                var planePos = new Vector2(plane.X, plane.Y);
                float dist = mouse.Position.DistanceTo(planePos);
                if (dist <= clickRadius && dist < closestDist)
                {
                    closest = plane;
                    closestDist = dist;
                }
            }

            if (closest != null)
            {
                SelectedAircraft = closest;
                OnAircraftSelected?.Invoke(closest);
                QueueRedraw();
            }
        }
    }

    public void ProcessCommand(string commandText)
    {
        var parts = commandText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    
        if (parts.Length < 3) 
        {
            GD.PrintErr("Format: CALLSIGN CMD VALUE (e.g., SQP112 HDG 180)");
            return;
        }

        string callsign = parts[0];
        string cmd = parts[1];
    
        var plane = _aircraft.Find(a => a.Callsign == callsign);
        if (plane == null)
        {
            GD.PrintErr($"Aircraft {callsign} not found on radar.");
            return;
        }

        if (!float.TryParse(parts[2], out float value))
        {
            GD.PrintErr("Invalid value. Must be a number.");
            return;
        }

        switch (cmd)
        {
            case "HDG":
                plane.TargetHeading = value; 
                GD.Print($"ATC: {callsign} turning to heading {value}");
                break;
            case "ALT":
                plane.TargetAltitude = (int)value; 
                GD.Print($"ATC: {callsign} descending/climbing to {value} ft");
                break;
            case "SPD":
                plane.TargetSpeed = value; 
                GD.Print($"ATC: {callsign} reducing/increasing speed to {value} knots");
                break;
            default:
                GD.PrintErr($"Unknown command: {cmd}. Use HDG, ALT, or SPD.");
                break;
        }
    }
    
    private void DrawWeatherRadar()
    {
        if (Main == null || Main.Weather.CloudCoverageOktas == 0) return; 

        int cellSize = 15; 
        float coverageThreshold = 1.0f - (Main.Weather.CloudCoverageOktas / 10.0f); 

        for (int x = 0; x < Size.X; x += cellSize)
        {
            for (int y = 0; y < Size.Y; y += cellSize)
            {
                float noiseVal = _weatherNoise.GetNoise2D(x + _noiseOffset.X, y + _noiseOffset.Y);
                noiseVal = (noiseVal + 1f) / 2f;
                
                if (noiseVal < coverageThreshold) continue;

                float intensity = (noiseVal - coverageThreshold) / (1f - coverageThreshold);

                Color cellColor;
                if (intensity > 0.7f)
                    cellColor = new Color(1, 0, 0, 0.3f);
                else if (intensity > 0.4f)
                    cellColor = new Color(1, 0.8f, 0, 0.2f);
                else
                    cellColor = new Color(0, 1, 0, 0.15f);

                DrawRect(new Rect2(x, y, cellSize, cellSize), cellColor);
            }
        }
    }
}