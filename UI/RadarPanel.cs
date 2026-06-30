using Godot;
using System;
using System.Collections.Generic;
using VectorATC.Core;

public partial class RadarPanel : Control
{
    private List<Aircraft> _aircraft = new List<Aircraft>();
    private TrafficSpawner _spawner;
    private int _previousAircraftCount = 0;

    public event Action<List<Aircraft>> OnTrafficUpdated;

    public override void _Ready()
    {
        SizeFlagsVertical = SizeFlags.ExpandFill;
        CustomMinimumSize = new Vector2(0, 300);
        ClipContents = true;

        _spawner = new TrafficSpawner(); 
    }

    public override void _Process(double delta)
    {
        _spawner.UpdateBounds(Size.X, Size.Y);
        
        _spawner.Update((float)delta, _aircraft);

        if (_aircraft.Count != _previousAircraftCount)
        {
            _previousAircraftCount = _aircraft.Count;
            OnTrafficUpdated?.Invoke(_aircraft);
        }

        foreach (var plane in _aircraft)
            plane.Move((float)delta);

        QueueRedraw();
    }

    public override void _Draw()
    {
        DrawRect(new Rect2(Vector2.Zero, Size), new Color("#0a0a0a"));

        foreach (var plane in _aircraft)
        {
            var position = new Vector2(plane.X, plane.Y);
            DrawCircle(position, 5f, Colors.LimeGreen);
            DrawString(ThemeDB.FallbackFont, position + new Vector2(8, -8),
                plane.Callsign, HorizontalAlignment.Left, -1, 14, Colors.LimeGreen);
        }
    }
}